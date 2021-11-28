using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Sprite[] LoadingAnimaiton;

    static string nextScene;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private void OnValidate()
    {
        LoadingAnimaiton = Resources.LoadAll<Sprite>("C88unity-chan_animation/A/png");
    }

    public Image image;
    private void Start()
    {
        if (nextScene == "")
            nextScene = "Loading";

        image = GetComponent<Image>();
    
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        Application.targetFrameRate = 16;
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        int frameCounter = 0;

        while (!op.isDone)
        {
            image.sprite = LoadingAnimaiton[Mathf.Min(frameCounter, LoadingAnimaiton.Length - 1)];

            if (frameCounter > LoadingAnimaiton.Length - 1)
            {
                Application.targetFrameRate = 144;
                op.allowSceneActivation = true;
                yield break;
            }

            yield return null;
            frameCounter += 1;
        }
    }
}