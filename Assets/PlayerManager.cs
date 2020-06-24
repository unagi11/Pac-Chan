using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int CoinNumber = 0;
    public int Score;

    private void Awake()
    {
        instance = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            SceneManager.LoadScene("GameOver");
    }

    public void CoinUp()
    {
        CoinNumber++;
//        Debug.LogError("Coin : " + PlayerManager.instance.CoinNumber);
        if (CoinNumber == 194)
            SceneManager.LoadScene("GameWin");
    }

}
