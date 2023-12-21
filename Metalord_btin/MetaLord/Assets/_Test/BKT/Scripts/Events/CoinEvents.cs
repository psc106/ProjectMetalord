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

    //코인 사용시 호출되는 이벤트
    public event Action<GunMode> onUseCoin;
    public void UseCoin(GunMode gunMode)
    {
        if (onUseCoin != null)
        {
            onUseCoin(gunMode);
        }
    }
}
