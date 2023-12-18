using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 상점 구매물품에 들어가는 스크립트
/// 231204 배경택
/// </summary>
public class StoreObject : MonoBehaviour
{
    protected int price;

    // 상점 UI버튼 누르면 실행되는 함수
    public virtual void BuyStoreObject() 
    {
        CoinManager.instance.UseCoin(price); //코인 반영
        //TODO 구매한 물품의 기능이 반영되도록 작성  //구매 기능 반영
    }

    // 구매 가능여부를 체크하는 함수
    public bool IsCanBuy()
    {
        if (CoinManager.instance.currentCoin >= price) return true;
        else return false;
    }
}
