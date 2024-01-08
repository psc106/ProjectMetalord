using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject[] effects;

    private void Awake()
    {
        instance = this;
    }
}

enum EffectList
{
    SmallStarGet=0,
    BigStarGet,
    ItemGet,
    GunMuzzle,
    GunExplosion
}
