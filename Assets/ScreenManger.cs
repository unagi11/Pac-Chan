using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScreenManger : MonoBehaviour
{
    private void Awake()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        Camera camera = Camera.main;
        Rect rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;

            canvasScaler.matchWidthOrHeight = 0f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;

            canvasScaler.matchWidthOrHeight = 1f;
        }
        camera.rect = rect;
    }
}
