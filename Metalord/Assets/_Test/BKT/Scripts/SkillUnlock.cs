using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlock : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private string skillDescription;
    [SerializeField] private string icon;

    public void BuySkill()
    {
        CoinManager.instance.UseCoin(price);
    }

}
