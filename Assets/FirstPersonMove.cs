using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMove : MonoBehaviour
{
    public static FirstPersonMove instance;

    public float moveSpeed = 3f;
    public float rotateSpeed = 10f;

    public float hor, joy_hor, ver, joy_ver;
    public Joystick joystick;

    [SerializeField]
    Vector3 front, right;
    float angle;

    Rigidbody _rigidbody;
    Camera _camera;

    private void Awake()
    {
        instance = this;
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        hor = Input.GetAxisRaw("Horizontal");
        joy_hor = joystick.Horizontal;
        ver = Input.GetAxisRaw("Vertical");
        joy_ver = joystick.Vertical;

        CameraMoveFunc(ver, joy_ver, hor, joy_hor);
    }
    
    void CameraMoveFunc(float ver, float hor)
    {
        Vector3 front = transform.position - _camera.transform.position;
        front = new Vector3(front.x, 0, front.z).normalized;//앞

        Vector3 right = (Quaternion.Euler(0, 90 , 0) * front).normalized;

        Vector3 direction = (front * ver + right * hor).normalized;

        _rigidbody.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

        if (hor != 0 || ver != 0)
            angle = Mathf.Acos(ver / Mathf.Sqrt(ver * ver + hor * hor)) / Mathf.PI * 180;

        if (hor < 0)
            angle = -angle;

         _rigidbody.MoveRotation( Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle + _camera.transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotateSpeed));

//        Debug.LogError(Mathf.Acos(ver / Mathf.Sqrt(ver * ver + hor * hor))/Mathf.PI * 180);

//        _rigidbody.MoveRotation(Quaternion.Euler(new Vector3(0, _camera.transform.rotation.eulerAngles.y, 0)));
    }

    void CameraMoveFunc(float ver1, float ver2, float hor1, float hor2)
    {
        float hor = 0, ver = 0;
        if (hor1 != 0)
            hor = hor1;
        else if (hor2 != 0)
            hor = hor2;
        if (ver1 != 0)
            ver = ver1;
        else if (ver2 != 0)
            ver = ver2;

        CameraMoveFunc(ver, hor);
    }


    void MoveFrontFunc(float ver)
    {
        Vector3 move = transform.forward * ver * moveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(transform.position + move);
    }

    void MoveFrontFunc(float ver1, float ver2)
    {
        float ver = 0;
        if (ver1 != 0)
            ver = ver1;
        else if (ver2 != 0)
            ver = ver2;

        MoveFrontFunc(ver);
    }

    void RotateFunc(float hor)
    {
        Quaternion rotate = Quaternion.Euler(0, transform.rotation.eulerAngles.y + hor * rotateSpeed * 10 * Time.deltaTime, 0);
        _rigidbody.MoveRotation(rotate);
    }

    void RotateFunc(float hor1, float hor2)
    {
        float hor = 0;
        if (hor1 != 0)
            hor = hor1;
        else if (hor2 != 0)
            hor = hor2;

        RotateFunc(hor);
    }
}
