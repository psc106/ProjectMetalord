using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상점오브젝트_무기상태
/// </summary>
public class Store_WeaponStatus : StoreObject
{

    [SerializeField] int[] stepCoin;
    [SerializeField] int[] stepSkillStatus;

    private int stepPointer;

    private void Awake()
    {
        stepPointer = 0;
    }

    protected override void BuyStoreObject()
    {
        price = stepCoin[stepPointer];
        base.BuyStoreObject();
    }
}
