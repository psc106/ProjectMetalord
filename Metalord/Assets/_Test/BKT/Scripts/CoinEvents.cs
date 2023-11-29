using System;
using UnityEngine;

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
    public event Action<int> onUseCoin;
    public void UseCoin(int coin)
    {
        if(onUseCoin != null)
        {
            onUseCoin(coin);
        }
    }
}
