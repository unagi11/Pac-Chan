using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(Animator))]
[RequireComponent(typeof(Effect))]
public class GhostAI : MonoBehaviour
{
    public Transform chaseTransform;
    public Transform scatterTransform;
    public Vector3 spawnVector;

    public float updateTime = 0.1f;
    public float updateAngle = 10f;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    public GhostSensor[] Sensors;

    public enum MoveDirection
    {
        Stop = 0, Front = 1, Left = 2, Back = 3, Right = 4
    }

    public enum GhostState
    {
        Chase, Scatter, Eaten, Frighten
    }

    Rigidbody _rigidbody;
    Collider _collider;
    Animator _animator;
    Effect _effect;

    private void Awake()
    {
        Sensors = GetComponentsInChildren<GhostSensor>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _effect = GetComponent<Effect>();
    }

    public MoveDirection currentDirection = MoveDirection.Stop;
    public GhostState currentState = GhostState.Chase;
    GhostState lateState = GhostState.Chase;

    Vector3 direction;
    Quaternion rotation;

    void Start()
    {
        StartCoroutine(WaitAndUpdate(updateTime));
        currentDirection = MoveDirection.Front; //초기화
        direction = transform.forward;
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0).normalized;
        spawnVector = transform.position;

        SetGhostScatter();
    }

    public void SetTarget(Transform _targetTransfrom) //Object 우선한다.
    {
        foreach (GhostSensor Sensor in Sensors)
            Sensor.targetTransform = _targetTransfrom;
    }

    public void SetTarget(Vector3 targetVector)
    {
        foreach (GhostSensor Sensor in Sensors)
        {
            Sensor.targetTransform = null;
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
        while (true)
        {
            if (Quaternion.Angle(rotation, transform.rotation) < updateAngle) // 회전중이 아닐때만 판단
                GhostAlgo.Invoke();

            yield return new WaitForSeconds(waitTime);
        }
    }

    Coroutine runningStateCoroutine = null; //StopCoroutine용

    [SerializeField]
    float chaseTime = 20f;
    [SerializeField]
    float scatterTime = 10f;
    [SerializeField]
    public static float frightenTime = 10f;
    [SerializeField]
    float eatenTime = 7f;

    public void SetGhostChase()
    {
        SetTarget(chaseTransform);//Chase화
        GhostAlgo = () => { GhostTargetAlgo(); };
        currentState = GhostState.Chase;//Chase화
        _effect.StopEffect();

        if (runningStateCoroutine != null) //억지로 바뀐경우면 runningStateCoroutine이 null이 아니다. Chase랑 Scatter은 억지로 바뀌어도 딱히 할게 없음.
            StopCoroutine(runningStateCoroutine);
        runningStateCoroutine = StartCoroutine(Function.CoInvoke(chaseTime, () =>
        {
            runningStateCoroutine = null; // 상태가 끝까지 갔다는 표시로 Coroutine을 null로 초기화해준다.
            SetGhostScatter();
        }));
    }
    public void SetGhostScatter()
    {
        if (scatterTime > 2f) //scatter 상태가 호출 될때마다 줄어든다.
            scatterTime -= 2f;

        SetTarget(scatterTransform);//Scatter화
        GhostAlgo = () => { GhostTargetAlgo(); };
        currentState = GhostState.Scatter;//Scatter화
        _effect.StopEffect();

        if (runningStateCoroutine != null) //억지로 바뀐경우면 runningStateCoroutine이 null이 아니다. Chase랑 Scatter은 억지로 바뀌어도 딱히 할게 없음.
            StopCoroutine(runningStateCoroutine);
        runningStateCoroutine = StartCoroutine(Function.CoInvoke(scatterTime, () =>
        {
            runningStateCoroutine = null; // 상태가 끝까지 갔다는 표시로 Coroutine을 null로 초기화해준다.
            SetGhostChase();
        }));
    }

    public void SetGhostFrighten()
    {
        if (currentState == GhostState.Eaten) // Eaten상태였을때는 Frighten되지 않습니다. 
            return;
        else if (currentState == GhostState.Chase || currentState == GhostState.Scatter) // Chase상태 혹은 Scatter상태였더라면 이전 상태를 기억합니다.
            lateState = currentState;

//        Debug.LogError(name + " : SetGhostFrighten");

        GhostAlgo = () => { GhostRandomAlgo(); }; //행동 패턴을 랜덤 알고리즘으로 바꿉니다.
        currentState = GhostState.Frighten;
        _effect.StartEffect(Effect.FrightenEffect); //FrightenEffect를 생성합니다.

        if (runningStateCoroutine != null)
            StopCoroutine(runningStateCoroutine);
        runningStateCoroutine = StartCoroutine(Function.CoInvoke(frightenTime, () =>
        {
            _effect.StopEffect();
            if (lateState == GhostState.Chase) //이전 상태로 돌아간다.
            {
                runningStateCoroutine = null;
                SetGhostChase();
            }
            else if (lateState == GhostState.Scatter)
            {
                runningStateCoroutine = null;
                SetGhostScatter();
            }
            else
                Debug.LogError("LateState Error!!");
        }));
    }

    public void SetGhostEaten()
    {
        _collider.isTrigger = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        GhostAlgo = () => { };
        currentState = GhostState.Eaten;
        _effect.StartEffect(Effect.EatenEffect);
        _animator.SetBool("isDizzy", true);

        if (runningStateCoroutine != null)
            StopCoroutine(runningStateCoroutine); //Frighten Coroutine 제거해야함
        runningStateCoroutine = StartCoroutine(Function.CoInvoke(eatenTime, () => 
        {
            _collider.isTrigger = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            transform.position = spawnVector;
            _effect.StopEffect();
            _animator.SetBool("isDizzy", false);

            if (lateState == GhostState.Chase)
            {
                runningStateCoroutine = null;
                SetGhostChase();
            }
            else if (lateState == GhostState.Scatter)
            {
                runningStateCoroutine = null;
                SetGhostScatter();
            }
            else
                Debug.LogError("LateState Error!!");
        }));
    }

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
            if (Sensor.current != MapManager.MapObjectCategory.Wall && Sensor.current != MapManager.MapObjectCategory.Enemy && Sensor.SensorDirection != MoveDirection.Back)
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
            if (Sensor.current != MapManager.MapObjectCategory.Wall && Sensor.current != MapManager.MapObjectCategory.Enemy && Sensor.SensorDirection != MoveDirection.Back)
                tempSensors.Add(Sensor);

        if (tempSensors.Count == 0)
            currentDirection = MoveDirection.Back;
        else
            currentDirection = tempSensors[UnityEngine.Random.Range(0, tempSensors.Count)].SensorDirection;
    }

}
