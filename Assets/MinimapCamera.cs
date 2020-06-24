using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + new Vector3(0, 20, 0);
        transform.rotation = Quaternion.Euler(90, target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.z);
    }
}
