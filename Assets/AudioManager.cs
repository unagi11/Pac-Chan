using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BgmPlayer, SePlayer;

    public AudioClip GetCoinAudio, GetPowerAudio;

    AudioClip tempAudio = null;
    Coroutine BGMcoroutine = null;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySEOnce(AudioClip audioClip)
    {
        SePlayer.loop = false;
        SePlayer.clip = audioClip;
        SePlayer.Play();
    }

    public void PlaySELoop(AudioClip audioClip)
    {
        SePlayer.loop = true;
        SePlayer.clip = audioClip;
        SePlayer.Play();
    }

    public void ChangeBGM(AudioClip audioClip)
    {
        BgmPlayer.loop = true;
        BgmPlayer.clip = audioClip;
        BgmPlayer.Play();
    }

    public void ChangeBGM(AudioClip audioClip, float time)
    {
        if (tempAudio == null)
            tempAudio = BgmPlayer.clip;

        ChangeBGM(audioClip);

        if (BGMcoroutine != null)
            StopCoroutine(BGMcoroutine);

        BGMcoroutine = StartCoroutine(cinvoke(time, () => { 
            ChangeBGM(tempAudio);
            BGMcoroutine = null;
            tempAudio = null;
        }));
    }


    public static IEnumerator cinvoke(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

}
