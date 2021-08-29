using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManger : MonoBehaviour
{
    public RectTransform ScreenMaskRect;

    public float scaleheight;

    public int width = 16;
    public int height = 9;

    public static int myResolution = 480;

    private void Awake()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        scaleheight = (float)Screen.width / Screen.height / ((float)width / height);
        SetMyResoution();

        if (scaleheight < 1) // 화면의 비율에서 세로가 길다
            canvasScaler.matchWidthOrHeight = 0f;
        else // 화면의 비율에서 가로가 길다
            canvasScaler.matchWidthOrHeight = 1f;
    }
    public static void SetMyResoution() => SetMyResoution(myResolution);

    public static void SetMyResoution(int _height)
    {
        myResolution = _height;

        int _width = (int)(Screen.width * (_height / (float)Screen.height));
        Screen.SetResolution(_width, _height, true);
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
