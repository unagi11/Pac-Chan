using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityChan;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public TextMeshProUGUI LeftCoinUI;

    public int Score;

    [SerializeField]
    int NumOfScore;

    [SerializeField]
    GameObject[] Enemys;

    Effect _effect;

    private void Awake()
    {
        instance = this;
        _effect = GetComponent<Effect>();
    }

    private void Start()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        NumOfScore = GameObject.FindGameObjectsWithTag("Score").Length;
        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GhostAI ghostAI = collision.gameObject.GetComponent<GhostAI>();
            if(ghostAI.currentState == GhostAI.GhostState.Chase || ghostAI.currentState == GhostAI.GhostState.Scatter)
                SceneManager.LoadScene("GameOver");
            else if (ghostAI.currentState == GhostAI.GhostState.Frighten)
                collision.gameObject.GetComponent<GhostAI>().SetGhostEaten();
        }
    }

    public void GetScore()
    {
        GetScore(1);
    }

    public void UpdateUI()
    {
        LeftCoinUI.text = "<sprite=\"Cherry_one\" index=0> " + Score + " / " + NumOfScore;
    }

    public void GetScore(int score)
    {
        AudioManager.instance.PlaySEOnce(AudioManager.instance.GetCoinSE);
        Score += score;
        UpdateUI();
        if (Score == NumOfScore)
            SceneManager.LoadScene("GameWin");
    }

    Coroutine runningCoroutine;

    public void GetPower()
    {
        AudioManager.instance.PlaySEOnce(AudioManager.instance.GetPowerSE);
        AudioManager.instance.PlayBGM(AudioManager.instance.GetPowerBGM);

        _effect.StartEffect(Effect.PowerEffect);

        GetComponent<FaceUpdate>().OnCallChangeFace("smile");

        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(Function.CoInvoke(GhostAI.frightenTime, () =>
        {
            AudioManager.instance.PlayBGM(AudioManager.instance.NormalBGM);
            _effect.StopEffect();
            GetComponent<FaceUpdate>().OnCallChangeFace("default");
        }));

        foreach (GameObject enemy in Enemys)
            enemy.GetComponent<GhostAI>().SetGhostFrighten();
    }
}
