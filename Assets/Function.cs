using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function : MonoBehaviour
{
    public static IEnumerator CoInvoke(float time, Action action = null)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
