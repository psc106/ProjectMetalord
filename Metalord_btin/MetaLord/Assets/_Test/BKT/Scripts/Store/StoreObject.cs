using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상점 구매물품에 들어가는 스크립트
/// 231204 배경택
/// </summary>
public class StoreObject : MonoBehaviour
{
    protected int price;

    private GameObject cantBuyImage;

    protected virtual void Awake()
    {
        Debug.Log(" StoreObject ");
        cantBuyImage = Utility.FindChildObj(this.gameObject, "Image(CantBuy)");
        GameEventsManager.instance.coinEvents.onChangeCoin += ChangeButtonUI;       
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.coinEvents.onChangeCoin -= ChangeButtonUI;
    }

    // 상점 UI버튼 누르면 실행되는 함수
    public virtual void BuyStoreObject() 
    {
        CoinManager.instance.UseCoin(price); //코인 반영
        //TODO 구매한 물품의 기능이 반영되도록 작성  //구매 기능 반영
    }

    /// <summary>
    /// 버튼 변경
    /// </summary>
    protected void ChangeButtonUI(int temp)
    {
        if (IsCanBuy()) cantBuyImage.SetActive(false);
        else cantBuyImage.SetActive(true);
    }

    // 구매 가능여부를 체크하는 함수
    protected bool IsCanBuy()
    {
        if (CoinManager.instance.currentCoin >= price) return true;
        else return false;
    }
}
