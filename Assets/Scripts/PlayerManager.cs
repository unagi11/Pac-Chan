using System.Collections;
using TMPro;
using UnityChan;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public TextMeshProUGUI ScoreUI;
    public TextMeshProUGUI CoinUI;
    public TextMeshProUGUI ComboUI;
    public Scrollbar comboBar;
    public Image fill;
    public Gradient barGradient;

    public int MyScore = 0;
    public int MyCoins = 0;
    public int MyCombo = 0;

    int NumOfAllCoins;

    [SerializeField]
    GameObject[] Enemys;

    Effect _effect;

    [SerializeField, Range(0, 1)]
    public static float ComboMeter = 0;

    [SerializeField]
    float ComboDelayTime = 2f;

    public enum PlayerState
    { 
        Normal, Power, DIe
    }

    public static PlayerState currentState = PlayerState.Normal;

    Camera _camera;

    private void Awake()
    {
        instance = this;
        _effect = GetComponent<Effect>();
        _camera = Camera.main;
    }

    private void Start()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        NumOfAllCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GhostAI ghostAI = collision.gameObject.GetComponent<GhostAI>();
            if(ghostAI.currentState == GhostAI.GhostState.Chase || ghostAI.currentState == GhostAI.GhostState.Scatter)
            {
                GoogleManager.AddLeaderboard(MyScore);
                MyCombo = 0;
                ComboMeter = 0;
                StopAllCoroutines();
                SceneManager.LoadScene("GameOver");
            }
            else if (ghostAI.currentState == GhostAI.GhostState.Frighten)
            {
                collision.gameObject.GetComponent<GhostAI>().SetGhostEaten();
                AudioManager.instance.PlaySEOnce(AudioManager.instance.AttackGhostSE);
                GetCombo();
                GetScore(10000);
            }
        }
    }

    public void UpdateUI()
    {
        ScoreUI.text = "Score " + MyScore;
        CoinUI.text = "<sprite=\"Cherry_one\" index=0> " + MyCoins + " / " + NumOfAllCoins;
    }

    public void GetCoin()
    {
        AudioManager.instance.PlaySEOnce(AudioManager.instance.GetCoinSE);
        CoinUI.GetComponent<Animator>().SetTrigger("getCoin");
        MyCoins += 1;

        GetCombo();
        GetScore(100 + 10 * MyCombo);

        if (MyCoins == NumOfAllCoins)
            SceneManager.LoadScene("GameWin");
    }

    public void GetCombo()
    {
        ComboUI.GetComponent<Animator>().SetTrigger("getCombo");
        MyCombo += 1;
        ComboMeter = 1f;
    }

    public void GetScore(int value)
    {
        MyScore += value;
        UpdateUI();
    }

    Coroutine runningCoroutine;

    public void GetPower()
    {
        AudioManager.instance.PlaySEOnce(AudioManager.instance.GetPowerSE);
        AudioManager.instance.PlayBGM(AudioManager.instance.GetPowerBGM);

        _effect.StartEffect(Effect.PowerEffect);

        GetComponent<FaceUpdate>().OnCallChangeFace("smile");

        currentState = PlayerState.Power;

        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        runningCoroutine = StartCoroutine(Function.CoInvoke(GhostAI.frightenTime, () =>
        {
            AudioManager.instance.PlayBGM(AudioManager.instance.NormalBGM);
            _effect.StopEffect();
            GetComponent<FaceUpdate>().OnCallChangeFace("default");

            currentState = PlayerState.Normal;
        }));

        foreach (GameObject enemy in Enemys)
            enemy.GetComponent<GhostAI>().SetGhostFrighten();
    }

    private void Update() {
    
        if (ComboMeter <= 0f)
        {
            MyCombo = 0;
            ComboMeter = 0f;
        }
        else
            ComboMeter -= Time.deltaTime / ComboDelayTime;

        comboBar.size = ComboMeter;
        ComboUI.text = MyCombo + " Combo";

        Color color = barGradient.Evaluate(((MyCombo - 1) + ComboMeter)/100);

        ComboUI.color = color;
        fill.color = color;
        CoinUI.color = color;
        ScoreUI.color = color;

    }

    // IEnumerator ComboCoroutine()
    // {
    //     while (true)
    //     {
    //         if (ComboMeter <= 0f)
    //         {
    //             MyCombo = 0;
    //             ComboMeter = 0f;
    //         }
    //         else
    //             ComboMeter -= Time.deltaTime / ComboDelayTime;

    //         comboBar.size = ComboMeter;
    //         ComboUI.text = MyCombo + " Combo";

    //         Color color = barGradient.Evaluate(((MyCombo - 1) + ComboMeter)/100);

    //         ComboUI.color = color;
    //         fill.color = color;
    //         CoinUI.color = color;
    //         ScoreUI.color = color;

    //         yield return null;
    //     }
    // }
}
