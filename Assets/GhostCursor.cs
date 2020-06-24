using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCursor : MonoBehaviour
{
    public float outRange = 5f;
    public float cursorRange = 4.5f;

    public GameObject target;
    public GameObject player;

    public Vector3 direction;
    public float angle;

    SpriteRenderer sprite;
    Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        startColor = sprite.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction = target.transform.position - player.transform.position;//player->target

        if (direction.magnitude > outRange)
            sprite.color = startColor;
        else
            sprite.color = Color.clear;

        angle = Vector3.SignedAngle(player.transform.forward, direction, Vector3.up);
        transform.rotation = Quaternion.Euler(new Vector3(90, player.transform.rotation.eulerAngles.y + angle, 0));

//        SetPosition(angle);
        float temp = (90 - (player.transform.rotation.eulerAngles.y + angle)) * Mathf.Deg2Rad;
        transform.position = player.transform.position + new Vector3(Mathf.Cos(temp) * cursorRange, 8, Mathf.Sin(temp) * cursorRange);
    }

    void SetPosition(float angle)
    {
        while (angle < 0 || angle > 360)
        {
            if (angle > 360)
                angle -= 360;
            if (angle < 0)
                angle += 360;
        }

        float temp = (90 - (player.transform.rotation.eulerAngles.y + angle)) * Mathf.Deg2Rad;

        if (angle > 315 || angle <= 45)
            transform.position = player.transform.position + new Vector3(Mathf.Tan(temp) * cursorRange, 8, cursorRange);
        else if (angle > 45 && angle <= 135)
            transform.position = player.transform.position + new Vector3(cursorRange, 8, Mathf.Cos(temp) * cursorRange);
        else if (angle > 135 && angle <= 225)
            transform.position = player.transform.position + new Vector3(cursorRange, 8, Mathf.Cos(temp) * cursorRange);
        else
            transform.position = player.transform.position + new Vector3(cursorRange, 8, Mathf.Cos(temp) * cursorRange);
    }

}
