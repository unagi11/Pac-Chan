using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
//    public GameObject CameraPosition;

    public CameraJoystick cameraJoystick;
    
    public float followSpeed = 4f;
    public float rotateSpeed = 5f;

    public float angle_x = 0f;
    public float distance_x = 5f;
    public float distance_y = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        angle_x = cameraJoystick.angle_v;
    }

    void FixedUpdate()
    {
        float temp = (angle_x - 90) * Mathf.Deg2Rad;// 라디안화

//        transform.position = player.transform.position + new Vector3(Mathf.Cos(temp) * distance_x, distance_y, Mathf.Sin(temp) * distance_x);
        transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(Mathf.Cos(temp) * distance_x, distance_y, Mathf.Sin(temp) * distance_x), followSpeed * Time.deltaTime);
        transform.LookAt(player.transform);
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
        transform.LookAt(player.transform);
    }



}
