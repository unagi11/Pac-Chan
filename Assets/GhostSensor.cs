using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSensor : MonoBehaviour
{
    public GameObject targetObject;
    public Vector3 targetVector;
    public float targetDistance;

    public MapManager.MapObjectCategory current = MapManager.MapObjectCategory.Empty;
    public GhostAI.MoveDirection SensorDirection;

    private void Update()
    {
        if (targetObject != null)
            targetVector = targetObject.transform.position;
        
        targetDistance = (transform.position - targetVector).magnitude;
        //Debug.DrawLine(transform.position, targetVector);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            current = MapManager.MapObjectCategory.Wall;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        current = MapManager.MapObjectCategory.Empty;
    }

}
