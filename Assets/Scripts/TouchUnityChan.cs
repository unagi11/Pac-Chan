using System;
using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchUnityChan : MonoBehaviour
{
    public GameObject HeartParticle;
    public Transform particleTransform;

    Animator _animator;

    private void Start()
    {
        if (HeartParticle == null)
            HeartParticle = Resources.Load("Effect/Heart") as GameObject;
        _animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        Instantiate(HeartParticle, particleTransform.position, Quaternion.identity, transform);

        // AudioManager.instance.PlaySEOnce(AudioManager.instance.randomVoices[UnityEngine.Random.Range(0, AudioManager.instance.randomVoices.Length-1)]);

        _animator.SetBool("Next", true);
    }

}
