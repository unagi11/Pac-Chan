using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject PowerEffect;

    public int Score;

    [SerializeField]
    int NumOfScore;

    [SerializeField]
    GameObject[] Enemys;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        NumOfScore = GameObject.FindGameObjectsWithTag("Score").Length;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            SceneManager.LoadScene("GameOver");
    }

    public void GetScore()
    {
        GetScore(1);
    }

    public void GetScore(int score)
    {
        AudioManager.instance.PlaySEOnce(AudioManager.instance.GetCoinSE);
        Score += score;
        if (Score == NumOfScore)
            SceneManager.LoadScene("GameWin");
    }

    Coroutine runningCoroutine;
    GameObject currentEffect;

    public void GetPower()
    {
        AudioManager.instance.PlaySEOnce(AudioManager.instance.GetPowerSE);
        AudioManager.instance.PlayBGM(AudioManager.instance.GetPowerBGM);

        if (currentEffect != null)
            Destroy(currentEffect);
        currentEffect = Instantiate(PowerEffect, transform);

        GetComponent<FaceUpdate>().OnCallChangeFace("smile");

        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(Function.CoInvoke(GhostAI.frightenTime, () =>
        {
            AudioManager.instance.PlayBGM(AudioManager.instance.NormalBGM);
            Destroy(currentEffect);
            GetComponent<FaceUpdate>().OnCallChangeFace("default");
            Debug.LogError("10초 지남!");
        }));

        foreach (GameObject enemy in Enemys)
            enemy.GetComponent<GhostAI>().SetGhostFrighten();
    }
}
