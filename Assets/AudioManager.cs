using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BGMPlayer, SEPlayer;

    public AudioClip NormalBGM, GetCoinSE, GetPowerBGM, GetPowerSE;

    [SerializeField]
    public static float BGMVolume, SEVolume;

    [SerializeField]
    float BGMTime, PowerTime;

    Coroutine BGMcoroutine = null;

    private void Awake()
    {
        instance = this;
        BGMVolume = BGMPlayer.volume;
        SEVolume = SEPlayer.volume;
    }

    private void Start()
    {
        PlayBGM(NormalBGM);
    }

    public void PlaySEOnce(AudioClip audioClip)
    {
        SEPlayer.loop = false;
        SEPlayer.clip = audioClip;
        SEPlayer.Play();
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

        StartCoroutine(BGMChange(audioClip, 1f));
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

    private IEnumerator BGMChange(AudioClip audioClip, float fadeTime)
    {
        AudioSource audioSource = BGMPlayer;
        float volume = BGMPlayer.volume;

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

        // 바꿀 BGM이 기본 BGM이 아니면 시간을 기록해라.
        if (audioClip != NormalBGM)
            BGMTime = BGMPlayer.time;
        else
            PowerTime = BGMPlayer.time;

        BGMPlayer.loop = true;
        BGMPlayer.clip = audioClip;

        // 바꿀 BGM이 기본 BGM이라면 기록한 시간으로 다시 시작한다.
        if (audioClip == NormalBGM)
            BGMPlayer.time = BGMTime;
        else
            BGMPlayer.time = PowerTime;

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
