using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorsRotation : MonoBehaviour
{
    public GhostAI ghostAI;

    private void Start()
    {
        ghostAI = GetComponentInParent<GhostAI>();
    }

    private void Update()
    {
        transform.rotation = ghostAI.rotation;
    }
}
