using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameResult : MonoBehaviour
{
    public static bool isWin = false;

    public static int Score = 0;

    public GameObject win, defeat;

    public TextMeshProUGUI score, comment;

    private void Start() {

        win.SetActive(isWin);
        defeat.SetActive(!isWin);
        score.text = $"Score {Score}";

        if (isWin)
        {
            AudioManager.instance.PlaySEOnce(AudioManager.instance.WinSE);
            AudioManager.instance.PlaySEOnce(AudioManager.instance.WinVoice);

            comment.text = "Great Job! You are awesome!";
        }
        else
        {
            AudioManager.instance.PlaySEOnce(AudioManager.instance.DefeatSE);
            AudioManager.instance.PlaySEOnce(AudioManager.instance.DefeatVoice);

            comment.text = "You can do it! Try again!";
        }
    }
}
