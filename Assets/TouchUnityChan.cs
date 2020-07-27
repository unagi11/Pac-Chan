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

        _animator.SetBool("Next", true);
    }

}
