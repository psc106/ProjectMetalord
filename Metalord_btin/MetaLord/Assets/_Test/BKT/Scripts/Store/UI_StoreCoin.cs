using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_StoreCoin : MonoBehaviour
{
    protected TMP_Text coinText;

    protected virtual void Awake()
    {
        coinText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.coinEvents.onChangeCoin += ReflectCoinToUI;

        if(CoinManager.instance != null)
        {
            ReflectCoinToUI(CoinManager.instance.currentCoin);

        }
    }

    private void OnDisable()
    {
        GameEventsManager.instance.coinEvents.onChangeCoin -= ReflectCoinToUI;
    }

    protected virtual void ReflectCoinToUI(int coin)
    {
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }
        else
        {
            Debug.LogError("coinText를 찾을 수 없습니다!");
        }
    }
}
