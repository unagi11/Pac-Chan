using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public GameObject chaseObject;
    public Vector3 scatterVector;
    public Vector3 spawnVector;

    public float updateTime = 0.1f;
    public float updateAngle = 5f;
    public float moveSpeed = 2f;
    public float rotationSpeed = 6f;

    public GameObject FrightenEffect;
    public GameObject EatenEffect;

    public GhostSensor [] Sensors;

    /* 0 - front
     * 1 - left
     * 2 - back
     * 3 - right
     */

    /* Level 1 : 7, 20, 7, 20, 5, 20, 5, 무한
     * Level 2 ~ : 7, 20, 7, 20, 5, 무한
     * 
     */

    public enum MoveDirection
    {
        Stop = 0, Front = 1, Left = 2, Back = 3, Right = 4
    }

    public enum GhostState
    {
        Chase, Scatter, Eaten, Frighten
    }

    Rigidbody _rigidbody;

    private void Awake()
    {
        Sensors = GetComponentsInChildren<GhostSensor>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public MoveDirection currentDirection = MoveDirection.Stop;
    public GhostState currentState = GhostState.Chase;
    GhostState tempState = GhostState.Chase;

    Vector3 direction;
    Quaternion rotation;

    void Start()
    {
        StartCoroutine(WaitAndUpdate(updateTime));
        currentDirection = MoveDirection.Front; //초기화
        direction = transform.forward;
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0).normalized;
        spawnVector = transform.position;

        SetGhostChase();
    }

    public void SetTarget(GameObject _targetObject) //Object 우선한다.
    {
        foreach (GhostSensor Sensor in Sensors)
            Sensor.targetObject = _targetObject;
    }

    public void SetTarget(Vector3 targetVector)
    {
        foreach (GhostSensor Sensor in Sensors)
        {
            Sensor.targetObject = null;
            Sensor.targetVector = targetVector;
        }
    }

    private void FixedUpdate()
    {
        GhostMove();
    }

    Action GhostAlgo;

    IEnumerator WaitAndUpdate(float waitTime)
    {
//        Vector3 temp = transform.position;//판단할때마다 갱신
//        Vector3 lateVector = transform.position;//waitTime마다 갱신
        while (true)
        {
//            Debug.Log(Quaternion.Angle(rotation, transform.rotation));
            if (Quaternion.Angle(rotation, transform.rotation) < updateAngle) // 회전중이 아닐때만 판단
//            {
                GhostAlgo.Invoke();
//                temp = transform.position;
//            }
            
            /*if ((transform.position - temp).magnitude > 0.5) // 한칸 움직였을 경우 판단
            {
                Debug.LogError("GhostAlgo1");

                GhostAlgo();
                temp = transform.position;
            }
            else if ((lateVector - transform.position).magnitude < 0.1) // 벽에 박을때마다 판단 (정지 상태에 가까운 경우)
            {
                Debug.LogError("GhostAlgo2");
                GhostAlgo();
                temp = transform.position;
            }*/

            //Debug.LogError((lateVector - transform.position).magnitude);

//            lateVector = transform.position;
            yield return new WaitForSeconds(waitTime);
        }
    }

    Coroutine runningStateCoroutine = null; //StopCoroutine용

    [SerializeField]
    float chaseTime = 20f;
    [SerializeField]
    float scatterTime = 10f;

    public static float frightenTime = 10f;

    public void SetGhostChase()
    {
        SetTarget(chaseObject);//Chase화
        GhostAlgo = () => { GhostTargetAlgo(); };
        currentState = GhostState.Chase;//Chase화

        if (runningStateCoroutine != null) //억지로 바뀐경우면 runningStateCoroutine이 null이 아니다. Chase랑 Scatter은 억지로 바뀌어도 딱히 할게 없음.
            StopCoroutine(runningStateCoroutine);

        runningStateCoroutine = StartCoroutine(DelayAndAction(chaseTime, () =>
        {
            runningStateCoroutine = null; // 상태가 끝까지 갔다는 표시로 Coroutine을 null로 초기화해준다.
            SetGhostScatter();
        }));
    }
    public void SetGhostScatter()
    {
        if (scatterTime > 2f) //scatter 상태가 호출 될때마다 줄어든다.
            scatterTime -= 2f;

        SetTarget(scatterVector);//Scatter화
        GhostAlgo = () => { GhostTargetAlgo(); };
        currentState = GhostState.Scatter;//Scatter화

        if (runningStateCoroutine != null) //억지로 바뀐경우면 runningStateCoroutine이 null이 아니다. Chase랑 Scatter은 억지로 바뀌어도 딱히 할게 없음.
            StopCoroutine(runningStateCoroutine);

        runningStateCoroutine = StartCoroutine(DelayAndAction(scatterTime, () =>
        {
            runningStateCoroutine = null; // 상태가 끝까지 갔다는 표시로 Coroutine을 null로 초기화해준다.
            SetGhostChase();
        }));
    }

    GameObject currentEffect;

    public void SetGhostFrighten()
    {
        SetTarget(scatterVector); //수정요함 : Frighten Vector 혹은 Frighten Target

        GhostAlgo = () => { GhostRandomAlgo(); };

        tempState = currentState;
        currentState = GhostState.Frighten;

        if (currentEffect != null)
            Destroy(currentEffect);
        currentEffect = Instantiate(FrightenEffect, transform);

        if (runningStateCoroutine != null)
            StopCoroutine(runningStateCoroutine);

        runningStateCoroutine = StartCoroutine(DelayAndAction(frightenTime, () =>
        {
            Destroy(currentEffect);
            if (tempState == GhostState.Chase)
            {
                runningStateCoroutine = null;
                SetGhostChase();
            } else if (tempState == GhostState.Scatter)
            {
                runningStateCoroutine = null;
                SetGhostScatter();
            }
        }));
    }
    public void SetGhostEaten()
    {
        SetTarget(spawnVector);
        tempState = currentState;
        currentState = GhostState.Eaten;

        if (currentEffect != null)
            Destroy(currentEffect);
        currentEffect = Instantiate(EatenEffect, transform);

        if (runningStateCoroutine != null)
            StopCoroutine(runningStateCoroutine);

        runningStateCoroutine = StartCoroutine(DelayAndAction(frightenTime, () => //수정요함 : frightenTime 수정 -> 집으로 가면 Eaten풀리는거로
        {
            if (tempState == GhostState.Chase)
            {
                runningStateCoroutine = null;
                SetGhostChase();
            }
            else if (tempState == GhostState.Scatter)
            {
                runningStateCoroutine = null;
                SetGhostScatter();
            }
        }));
    }

    public IEnumerator DelayAndAction(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func.Invoke();
    }

    //예약
    //영구
    //수정

    public void GhostMove()
    {
        if (currentDirection == MoveDirection.Stop)
            return;
        else if (currentDirection == MoveDirection.Right)
        {
            direction = transform.right;
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 90, 0).normalized;
            currentDirection = MoveDirection.Front;
        }
        else if (currentDirection == MoveDirection.Left)
        {
            direction = -transform.right;
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 270, 0).normalized;
            currentDirection = MoveDirection.Front;
        }
        else if (currentDirection == MoveDirection.Back)
        {
            direction = -transform.forward;
            rotation = Quaternion.Euler(0, rotation.eulerAngles.y + 180, 0).normalized;
            currentDirection = MoveDirection.Front;
        }

        _rigidbody.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime));
    }

    public void GhostTargetAlgo()
    {
        List<GhostSensor> tempSensors = new List<GhostSensor>();

        foreach (GhostSensor Sensor in Sensors)
            if (Sensor.current != MapManager.MapObjectCategory.Wall && Sensor.SensorDirection != MoveDirection.Back)
                tempSensors.Add(Sensor);

        if (tempSensors.Count == 0)
        {
            currentDirection = MoveDirection.Back;
            return;
        }
        else
        {
            float tempDistance = tempSensors[0].targetDistance;
            GhostSensor direction = tempSensors[0];

            foreach (GhostSensor Sensor in tempSensors)
                if (Sensor.targetDistance < tempDistance)
                {
                    direction = Sensor;
                    tempDistance = Sensor.targetDistance;
                }
            Debug.DrawLine(direction.transform.position, direction.targetVector, Color.white, 0.5f);
            currentDirection = direction.SensorDirection;
        }
    }

    public void GhostRandomAlgo()
    {
        List<GhostSensor> tempSensors = new List<GhostSensor>();

        foreach (GhostSensor Sensor in Sensors)
            if (Sensor.current != MapManager.MapObjectCategory.Wall && Sensor.SensorDirection != MoveDirection.Back)
                tempSensors.Add(Sensor);

        if (tempSensors.Count == 0)
            currentDirection = MoveDirection.Back;
        else
            currentDirection = tempSensors[UnityEngine.Random.Range(0, tempSensors.Count)].SensorDirection;
    }

}
