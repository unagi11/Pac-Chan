using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

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
        AudioManager.instance.PlaySEOnce(AudioManager.instance.GetCoinAudio);
        Score += score;
        if (Score == NumOfScore)
            SceneManager.LoadScene("GameWin");
    }


    public void GetPower()
    {
        AudioManager.instance.ChangeBGM(AudioManager.instance.GetPowerAudio, GhostAI.frightenTime);

        foreach (GameObject enemy in Enemys)
            enemy.GetComponent<GhostAI>().SetGhostFrighten();
    }

}
