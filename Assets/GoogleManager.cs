using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{

    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
