using System;
using UnityEngine;

/// <summary>
/// 코인 이벤트
/// 231129_ 배경택
/// </summary>
public class CoinEvents
{
    public CoinEvents()
    {
        Debug.Log("CoinEvents 생성");
    }

    // 코인 얻었을때 호출되는 이벤트
    public event Action<int> onChangeCoin;
    public void ChangeCoin(int coin)
    {
        if(onChangeCoin != null)
        {
            onChangeCoin(coin);
        }
    }

    // 코인 사용시 호출되는 이벤트
    // TODO 이벤트 발생시, 아이템 구매되도록 _ 이쪽에서 이벤트 처리 하지 않을 수 있음
    //public event Action onUseCoin;
    //public void UseCoin()
    //{
    //    if(onUseCoin != null)
    //    {
    //        onUseCoin();
    //    }
    //}
}
