using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float Vertical = 0;
    public float Horizontal = 0;

    public float angle_v = 0;

    float angleLimit = 360f;

    [SerializeField]
    float angleSpeedDown = 10f;

    [SerializeField]
    float angleSpeedLimit = 5f;

    Vector2 first = Vector3.zero;

    Vector2 temp;

    public void OnDrag(PointerEventData eventData)
    {
        Horizontal = (temp.x - eventData.position.x) / angleSpeedDown;
        Vertical = (eventData.position.y - first.y) / angleSpeedDown;

        if (Mathf.Abs(Horizontal) > angleSpeedLimit)
            Horizontal = angleSpeedLimit * (Horizontal/Mathf.Abs(Horizontal));

        angle_v += Horizontal;
        temp = eventData.position;
   
        if (angle_v < 0)
        {
            angle_v %= angleLimit;
            angle_v += angleLimit;
        }
        else if (angle_v > 360)
            angle_v %= angleLimit;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        temp = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vertical = 0;
        first = Vector2.zero;
    }
}
