using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BgmPlayer, SePlayer;

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

    public void StopSE(AudioClip audioClip)
    {

    }



}
