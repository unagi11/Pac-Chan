using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject runningEffect;

    public static GameObject GetCoinEffect;
    public static GameObject GetStarEffect;
    public static GameObject PowerEffect;
    public static GameObject FrightenEffect;
    public static GameObject EatenEffect;
    public static GameObject SpawnEffect;

    private void Awake()
    {
        GetCoinEffect = Resources.Load("Effect/PickUpCherries", typeof(GameObject)) as GameObject;
        GetStarEffect = Resources.Load("Effect/PicUpStar", typeof(GameObject)) as GameObject;
        PowerEffect = Resources.Load("Effect/PowerEffect", typeof(GameObject)) as GameObject;
        FrightenEffect = Resources.Load("Effect/SkullEffect", typeof(GameObject)) as GameObject;
        EatenEffect = Resources.Load("Effect/DizzyEffect", typeof(GameObject)) as GameObject;
        SpawnEffect = Resources.Load("Effect/ResurrectionLight", typeof(GameObject)) as GameObject;
    }

    public void StartEffect(GameObject effect)
    {
        StopEffect();
        runningEffect = Instantiate(effect, transform);
    }

    public void StartEffect(GameObject effect, float time)
    {
        StopEffect();
        runningEffect = Instantiate(effect, transform);
        Function.CoInvoke(time, () => { StopEffect(); });
    }

    public void StopEffect()
    {
        if (runningEffect != null)
            Destroy(runningEffect);
    }


}
