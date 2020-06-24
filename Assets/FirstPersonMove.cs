using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMove : MonoBehaviour
{
    public static FirstPersonMove instance;

    public float moveSpeed = 3f;
    public float rotateSpeed = 10f;

    public float hor, joy_hor, ver;
    public Joystick joystick;

    Rigidbody _rigidbody;

    private void Awake()
    {
        instance = this;
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
        //        ver = Input.GetAxisRaw("Vertical");

        MoveFunc(1);
        RotateFunc(hor, joy_hor);
    }

    void MoveFunc(float ver)
    {
        Vector3 move = transform.forward * ver * moveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(transform.position + move);
    }

    void RotateFunc(float hor1, float hor2)
    {
        float hor = 0;
        if (hor1 != 0)
            hor = hor1;
        else if (hor2 != 0)
            hor = hor2;

        Quaternion rotate = Quaternion.Euler(0, transform.rotation.eulerAngles.y + hor * rotateSpeed * 10 * Time.deltaTime, 0);
        _rigidbody.MoveRotation(rotate);
    }
}
