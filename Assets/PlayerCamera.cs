using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    public GameObject CameraPosition;
    public float followSpeed = 4f;
    public float rotateSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, CameraPosition.transform.position, followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, CameraPosition.transform.rotation, rotateSpeed * Time.deltaTime);
    }
}
