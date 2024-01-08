using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField, Range(20, 100)]
    int poolSize;

    public static EffectManager instance;
    public GameObject[] effects;

    private Dictionary<EffectList, Stack<GameObject>> effectPool;
    private Stack<GameObject> muzzleEffectPool;
    private Stack<GameObject> explosionEffectPool;

    private void Awake()
    {
        instance = this;

        muzzleEffectPool = new Stack<GameObject>();
        explosionEffectPool = new Stack<GameObject>();
        effectPool = new Dictionary<EffectList, Stack<GameObject>>
        {
            { EffectList.GunMuzzle, muzzleEffectPool },
            { EffectList.GunExplosion, explosionEffectPool }
        };

        InitEffectPool(EffectList.GunMuzzle);
        InitEffectPool(EffectList.GunExplosion);
    }

    private void InitEffectPool(EffectList effect)
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject effectObj = Instantiate(effects[(int)effect]);
            effectObj.SetActive(false);
            effectPool[effect].Push(effectObj);
        }
    }


    public GameObject GetEffect(EffectList effect)
    {
        if (effectPool[effect].Count == 0) 
        { 
            InitEffectPool(effect);
        }
        GameObject effectObj = effectPool[effect].Pop();
        effectObj.SetActive(true);
        effectObj.GetComponent<ParticleSystem>().Play();
        return effectObj;
    }

    public void ReturnEffectPool(EffectList effect, GameObject effectObj)
    {
        effectObj.SetActive(false);
        effectPool[effect].Push(effectObj);
    }
}

public enum EffectList
{
    SmallStarGet=0,
    BigStarGet,
    ItemGet,
    GunMuzzle,
    GunExplosion
}
