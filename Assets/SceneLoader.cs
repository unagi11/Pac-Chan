using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    protected static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SceneLoader>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    [SerializeField] private CanvasGroup sceneLoaderCanvasGroup;

    private string loadSceneName;

    public Sprite[] LoadingAnimaiton;

    private void OnValidate()
    {
        LoadingAnimaiton = Resources.LoadAll<Sprite>("C88unity-chan_animation/A/png");
    }

    public static SceneLoader Create()
    {
        var SceneLoaderPrefab = Resources.Load<SceneLoader>("SceneLoader");
        return Instantiate(SceneLoaderPrefab);
    }
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        image = GetComponentInChildren<Image>();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = sceneName; 
        StartCoroutine(Load(sceneName));
    }

    public Image image;

    private int midFrame = 14;

    private IEnumerator Load(string sceneName)
    {
        Application.targetFrameRate = 14;
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        int frameCounter = 0;

        while (!op.isDone)
        {
            image.sprite = LoadingAnimaiton[Mathf.Min(frameCounter, midFrame)];

            if (frameCounter > midFrame)
            {
                op.allowSceneActivation = true;
                yield break;
            }

            yield return null;
            frameCounter++;
        }
    }
    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        int frameCounter = midFrame;

        while (frameCounter < LoadingAnimaiton.Length)
        {
            image.sprite = LoadingAnimaiton[frameCounter];
            yield return null;
            frameCounter++;
        }
        
        Application.targetFrameRate = 60;
        gameObject.SetActive(false);
    }
}
