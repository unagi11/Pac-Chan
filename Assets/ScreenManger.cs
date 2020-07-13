using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScreenManger : MonoBehaviour
{
    public RectTransform ScreenMaskRect;

    private void Awake()
    {

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        Camera camera = Camera.main;
//        Rect rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
//        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
//            rect.height = scaleheight;
//            rect.y = (1f - scaleheight) / 2f;

            canvasScaler.matchWidthOrHeight = 0f;
        }
        else
        {
//            rect.width = scalewidth;
//            rect.x = (1f - scalewidth) / 2f;

            canvasScaler.matchWidthOrHeight = 1f;
        }
//        camera.rect = rect;
    }

    private void Start()
    {
//        EndCircleAnimation();
    }

    public void EndCircleAnimation()
    {
        StartCoroutine(RectTimeScaler(ScreenMaskRect, 300f, 300f, 2.5f, () => {
            StartCoroutine(Function.CoInvoke(3f, () =>
            {
                StartCoroutine(RectTimeScaler(ScreenMaskRect, 0.01f, 0.01f, 2.5f));
            }));
        }));
    }

    IEnumerator RectTimeScaler(RectTransform rectTransform, float width, float height, float time, Action next = null)
    {
        Rect _rect = rectTransform.rect;
        float dWidth = width - rectTransform.rect.width;
        float dHeight = height - rectTransform.rect.height;
        float timer = 0;

        Debug.LogError(_rect);

        while (timer < time)
        {
            _rect.width += Time.deltaTime / time * dWidth;
            _rect.height += Time.deltaTime / time * dHeight;
            timer += Time.deltaTime;

            rectTransform.sizeDelta = new Vector2(_rect.width, _rect.height);

            if (timer > time)
            {
                timer = time;
                rectTransform.sizeDelta = new Vector2(width, height);
            }

            yield return null;
        }

        next?.Invoke();
    }


}
