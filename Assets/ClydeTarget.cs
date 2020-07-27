using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeTarget : MonoBehaviour
{
    public GameObject Clyde;
    public Transform BlinkyTarget;

    Transform ClydeTransform;
    GhostAI ClydeAI;

    private void Start()
    {
        ClydeTransform = Clyde.transform;
        ClydeAI = Clyde.GetComponent<GhostAI>();
    }

    private void Update()
    {
        if (Vector3.Distance(ClydeTransform.position, BlinkyTarget.position) > 8f)
            transform.position = BlinkyTarget.position;
        else
            transform.position = ClydeAI.scatterTransform.position;
    }

}
