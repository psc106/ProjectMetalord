using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ExplainPage
{
    Panel = 0,    
    PC,
    Shoot,
    Move,
    Coin,
    Upgrade,
    GetItem,
    Record,
    NPC,
    NOTE,
}

/// <summary>
/// 설명 창
/// 231226 배경택
/// </summary>
public class ExplainCanvas : MonoBehaviour
{    
    private int _pageIndex;
    private int pageIndex
    {
        get
        {
            return _pageIndex;
        }

        set
        {
            transform.GetChild(_pageIndex).gameObject.SetActive(false);
            _pageIndex = value;
            transform.GetChild(_pageIndex).gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        pageIndex = (int)ExplainPage.PC;
        transform.GetChild((int)ExplainPage.NOTE).gameObject.SetActive(true);
    }

    public void Click_PC()
    {
        pageIndex = (int)ExplainPage.PC;        
    }

    public void Click_Shoot()
    {
        pageIndex = (int)ExplainPage.Shoot;
    }

    public void Click_Move()
    {
        pageIndex = (int)ExplainPage.Move;
    }

    public void Click_Coin()
    {
        pageIndex = (int)ExplainPage.Coin;
    }

    public void Click_Upgrade()
    {
        pageIndex = (int)ExplainPage.Upgrade;
    }

    public void Click_GetItem()
    {
        pageIndex = (int)ExplainPage.GetItem;
    }

    public void Click_Record()
    {
        pageIndex = (int)ExplainPage.Record;
    }

    public void Click_NPC()
    {
        pageIndex = (int)ExplainPage.NPC;
    }

    // 임시 소리
    public void CanUseSound()
    {
        // 사운드 추가       
        SoundManager.instance.PlaySound(GroupList.UI, (int)UISoundList.Can_BuySound);
    }
}