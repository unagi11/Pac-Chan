using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 deltaMove;
    public Vector2 move;

    [SerializeField, Range(180f, 360f)]
    float rotationSpeed = 270f; // 90 per second

    float screenWidth = Screen.width;
    float screenHeight = Screen.height;

    Vector2 tempMove = Vector2.zero;

    public void OnDrag(PointerEventData eventData)
    {
        deltaMove.x = (eventData.position.x - tempMove.x) * rotationSpeed / screenWidth;
        deltaMove.y = (eventData.position.y - tempMove.y) * rotationSpeed / screenHeight;

        tempMove = eventData.position;
        move += deltaMove;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tempMove = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        deltaMove = Vector2.zero;
    }
}
