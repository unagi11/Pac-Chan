using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public GameObject target;
    public GameObject mainCamera;

    private void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player");
        if (mainCamera == null)
            mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + new Vector3(0, 10, 0);
        transform.rotation = Quaternion.Euler(90, mainCamera.transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z);
    }
}
