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
    [SerializeField] protected int price;
    //[SerializeField] protected string skillDescription;
    //[SerializeField] protected string icon;

    private void BuyStoreObject() // 상점 UI버튼 누르면 실행되는 함수
    {
        CoinManager.instance.UseCoin(price); //코인 반영
        //TODO 구매한 물품의 기능이 반영되도록 작성  //구매 기능 반영
    }
}
