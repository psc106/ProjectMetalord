using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Coin : MonoBehaviour
{
    private TMP_Text coinText;

    private void Awake()
    {
        coinText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.coinEvents.onChangeCoin += ReflectCoinToUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.coinEvents.onChangeCoin -= ReflectCoinToUI;
    }

    /// <summary>
    /// UI에 코인 개수 반영하는 함수
    /// 231130_배경택
    /// </summary>
    /// <param name="coin"> 반영할 코인 </param>
    private void ReflectCoinToUI(int coin)
    {
        coinText.text = coin.ToString();
    }
}
