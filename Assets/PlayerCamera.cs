using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Transform focus;
    //    public GameObject CameraPosition;

    public CameraJoystick cameraJoystick;

    [SerializeField, Range(0.1f, 10f)]
    float followSpeed = 5f;
    [SerializeField, Range(0.1f, 10f)]
    float rotateSpeed = 5f;

    public float orbitAngle = 0f;

    [SerializeField, Range(0f, 5f)]
    float distance = 5f;
//    [SerializeField, Range(0f, 5f)]
//    float distance_height = 5f;

    [SerializeField, Min(0f)]
    float focusRadius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (focus == null)
            focus = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
//        orbitAngle = cameraJoystick.angle_v;
    }

    [SerializeField]
    Vector2 orbitAngles = new Vector2(45f, 0f);

    private void FixedUpdate ()
    {
        UpdateFocusPoint();
        Quaternion lookRotation; // 보는 각도
//        if ()
        {
            ManualRotation();
            ConstrainAngles();
            lookRotation = Quaternion.Euler(orbitAngles);
        }
//        else
        {
//            lookRotation = transform.localRotation;
        }
        Vector3 lookDirection = lookRotation * Vector3.forward; // 0,0,1에서 LookRotation 만큼 회전합니다.
        Vector3 lookPosition = focusPoint - lookDirection * distance; // 대상으로부터 보는 방향과 거리로 떨어진다.
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, lookPosition, followSpeed * Time.deltaTime), Quaternion.Lerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime));
        //        UpdateOrbit(orbitAngle);
        //        transform.LookAt(focus.transform);
    }

    [SerializeField]
    Vector3 focusPoint;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;


    private void Awake()
    {
        instance = GetComponent<PlayerCamera>();

        focusPoint = focus.position;
        transform.localRotation = Quaternion.Euler(orbitAngles);
    }

    [SerializeField, Range(-89f, 89f)]
    float minVerticalAngle = 30f, maxVerticalAngle = 60f;

    Vector2 lateMove; // !!!! 카메라 조이스틱에서 드래그중에 손을 계속대고 있으면 deltaMove가 유지되는 버그때문에 넣은 변수 !!!!

    bool ManualRotation ()// 입력이 있으면 true, 없으면 false
    {
        Vector2 input = new Vector2(-cameraJoystick.deltaMove.y, cameraJoystick.move.x);
        const float e = 0.001f;
        if(Vector2.Distance(lateMove, input) > e)
        {
            orbitAngles.x += input.x;
            orbitAngles.y = input.y;
            lateMove = input;
            return true;
        }
        return false;
    }

    void ConstrainAngles()
    {
        if (orbitAngles.x < minVerticalAngle)
        {
            orbitAngles.x = minVerticalAngle;
        } else if (orbitAngles.x >= maxVerticalAngle)
        {
            orbitAngles.x = maxVerticalAngle;
        }

        if (orbitAngles.y < 0f)
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
        }
    }

    private void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = focus.position; // targetPoint = 캐릭터의 현재 위치
        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                //focusPoint = Vector3.Lerp(targetPoint, focusPoint, focusRadius / distance);
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t); // focusPoint = 현재 카메라의  위치
        } else
            focusPoint = targetPoint;
    }

    void UpdateOrbit(float angle)
    {
        angle = (angle - 90) * Mathf.Deg2Rad;// 라디안화
        //transform.position = focus.transform.position + new Vector3(Mathf.Cos(angle) * distance_width, distance_height, Mathf.Sin(angle) * distance_width);
        //transform.position = Vector3.Lerp(transform.position, focus.transform.position + new Vector3(Mathf.Cos(angle) * distance_x, distance_y, Mathf.Sin(angle) * distance_x), followSpeed * Time.deltaTime);
    }

    /*    
     *  void CameraFunc1()
     *  {
     *      transform.position = Vector3.Lerp(transform.position, CameraPosition.transform.position, followSpeed * Time.deltaTime);
     *      transform.rotation = Quaternion.Slerp(transform.rotation, CameraPosition.transform.rotation, rotateSpeed * Time.deltaTime);
     *  }
     */

    void CameraFunc2()
    {
//        transform.position = Vector3.Lerp(transform.position, player.transform.position + CameraFirstPosition, followSpeed * Time.deltaTime);
        transform.LookAt(focus.transform);
    }



}
