using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseCube : MonoBehaviour
{
    public float noise;
    public float refinement = 0.1f;
    public float SpeedMultiflier = 10f;

    public float Minimum = 0.5f;
    public float UpMultiflier = 0.4f;

    private void Start()
    {
        noise = Mathf.PerlinNoise(transform.position.x * refinement, transform.position.z * refinement);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Abs(Mathf.Sin(Time.time + noise * SpeedMultiflier)) * UpMultiflier + Minimum, transform.position.z);
    }
}
