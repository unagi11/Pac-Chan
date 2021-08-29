using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyTarget : MonoBehaviour
{
    public Transform twoFrontTransform;
    public Transform blinkyTransform;

    // Update is called once per frame
    void Update()
    {
        Vector3 result = twoFrontTransform.position - (blinkyTransform.position - twoFrontTransform.position);
        transform.position = new Vector3(result.x, 0.5f, result.z);
    }
}
