using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{
    private static GoogleManager instance = null;

    [SerializeField]
    TextMeshProUGUI LoginText;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }

        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Login();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    LoginText?.SetText("Login Successed\nName : " + Social.localUser.userName);
                } else
                {
                    LoginText?.SetText("Login Failed\nName : Guest");
                }
            });
        }
    }

    public void Logout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        LoginText?.SetText("Login Failed\nName : Guest");
    }

    public static void AddLeaderboard(int Score)
    {
        if (!Social.localUser.authenticated)
            Social.ReportScore(Score, GPGSIds.leaderboard_score, (bool isSuccess) => { });
    }
}
