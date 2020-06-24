using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 3f;

    float hor, ver;
    Vector3 movement;
    Rigidbody _rigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        hor = Input.GetAxisRaw("Horizonta   l");
        ver = Input.GetAxisRaw("Vertical");

        MoveFunc(hor, ver);
        RotateFunc();
    }

    void MoveFunc(float hor, float ver)
    {
        movement.Set(hor, 0, ver);
        movement = movement.normalized * moveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(transform.position + movement);
//        transform.position = transform.position + movement;
    }

    void RotateFunc()
    {
        if (hor != 0 || ver != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
