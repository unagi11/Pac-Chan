using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public void TouchDown()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void TouchUp()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LisenceURL()
    {
        Application.OpenURL("https://unity-chan.com/contents/license_en/");
    }
}
