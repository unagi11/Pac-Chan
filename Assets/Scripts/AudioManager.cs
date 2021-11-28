using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BGMPlayer, SEPlayer;

    public AudioClip NormalBGM,  AnimeSongBGM, GetPowerBGM;

    public AudioClip GetCoinSE, GetPowerSE, AttackGhostSE, WinSE, DefeatSE;

    public AudioClip WinVoice, DefeatVoice;

    public AudioClip [] randomVoices;

    [SerializeField]
    public static float BGMVolume, SEVolume;

    [SerializeField]
    float DefaultBGMTime, PowerBGMTime;

    Coroutine BGMcoroutine = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            SEVolume = SEPlayer.volume;
            BGMVolume = BGMPlayer.volume;
            BGMPlayer.loop = true;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (Scene s, LoadSceneMode l) => {
            if (s.name == "PlayScene")
                BGMPlayer.clip = NormalBGM;
            else if (s.name == "TitleScene")
                BGMPlayer.clip = AnimeSongBGM;
            else
                BGMPlayer.clip = null;

            BGMPlayer.Play();
        };

        if (SceneManager.GetActiveScene().name == "PlayScene")
            BGMPlayer.clip = NormalBGM;
        else if (SceneManager.GetActiveScene().name == "TitleScene")
            BGMPlayer.clip = AnimeSongBGM;

        BGMPlayer.Play();
    }

    public void PlaySEOnce(AudioClip audioClip)
    {
        // SEPlayer.loop = false;
        // SEPlayer.clip = audioClip;
        SEPlayer.PlayOneShot(audioClip);
    }

    public void PlaySELoop(AudioClip audioClip)
    {
        SEPlayer.loop = true;
        SEPlayer.clip = audioClip;
        SEPlayer.Play();
    }

    public void PlayBGM(AudioClip audioClip)
    {
        //     if (BGMPlayer.clip == audioClip)
        //         return;

        if (CurCoroutine != null)
            StopCoroutine(CurCoroutine);

        CurCoroutine = BGMChange(audioClip, 0.3f);

        StartCoroutine(CurCoroutine);
    }

    // public void PlayBGM(AudioClip audioClip, float time)
    // {
    //     PlayBGM(audioClip);

    //     if (BGMcoroutine != null)
    //         StopCoroutine(BGMcoroutine);

    //     BGMcoroutine = StartCoroutine(Function.CoInvoke(time, () => { 
    //         PlayBGM(NormalBGM);
    //         BGMcoroutine = null;
    //     }));
    // }

    private IEnumerator CurCoroutine = null;

    private IEnumerator BGMChange(AudioClip audioClip, float fadeTime)
    {
        AudioSource audioSource = BGMPlayer;
        float volume = BGMVolume;

        // Fade out
        while (audioSource.volume > 0)
        {
            audioSource.volume -= volume * Time.deltaTime / fadeTime;

            if (audioSource.volume <= 0.005f)
            {
                audioSource.volume = 0;
                break;
            }

            yield return null;
        }


        // 시간을 기록하고
        if (BGMPlayer.clip == NormalBGM)
            DefaultBGMTime = BGMPlayer.time;
        else if (BGMPlayer.clip == GetPowerBGM)
            PowerBGMTime = BGMPlayer.time;

        BGMPlayer.clip = audioClip;

        // 나중에 다시 그부분부터 시작한다.
        if (audioClip == NormalBGM)
            BGMPlayer.time = DefaultBGMTime;
        else if (audioClip == GetPowerBGM)
            BGMPlayer.time = PowerBGMTime;

        BGMPlayer.Play();

        // Fade in
        while (audioSource.volume < volume)
        {
            audioSource.volume += volume * Time.deltaTime / fadeTime;

            if (audioSource.volume >= volume - 0.005f)
            {
                audioSource.volume = volume;
                break;
            }

            yield return null;
        }
    }


    public static IEnumerator AudioFadeIn(AudioSource audioSource, float fadeTime, float volume = 1f, Action nextEvent = null) // 소리 활성화
    {
        audioSource.volume = 0;

        while (audioSource.volume < volume)
        {
            audioSource.volume += (Time.deltaTime / fadeTime) * volume;
            Debug.Log($"AudioFadeIn volume : {audioSource.volume}");

            if (audioSource.volume >= volume)
                audioSource.volume = volume;

            yield return null;
        }

        nextEvent?.Invoke();
    }

    public static IEnumerator AudioFadeOut(AudioSource audioSource, float fadeTime, float volume = 1f, Action nextEvent = null) // 소리 비활성화
    {
        audioSource.volume = volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= (Time.deltaTime / fadeTime) * volume;
            Debug.Log($"AudioFadeOut volume : {audioSource.volume}");

            if (audioSource.volume <= 0)
                audioSource.volume = 0;

            yield return null;
        }

        nextEvent?.Invoke();
    }

}
