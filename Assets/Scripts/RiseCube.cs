using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseCube : MonoBehaviour
{
    public float noise;
    float refinement = 0.1f;

    [SerializeField, Range(-10, 10)]
    float minimum_y = 1, maximum_y = 2;

    private void Start()
    {
        noise = Mathf.PerlinNoise(transform.position.x * refinement, transform.position.z * refinement) * 10;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, minimum_y + (maximum_y - minimum_y) / 2 + (maximum_y - minimum_y) / 2  * Mathf.Sin(Time.time + noise), transform.position.z);
    }
}
