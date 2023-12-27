using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 코인 스크립트
/// 231219 배경택
/// </summary>
public class Coin : MonoBehaviour
{
    [SerializeField] private CoinType mytype; // 인스펙터창에서 코인 타입 선택

    // 코인 증가량
    private const int SMALL_COIN_VALUE = 5; // 작은코인 값
    private const int BIG_COIN_VALUE = 20; // 큰 코인 값

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("코인 먹었습니다"); 
            if (mytype == CoinType.SMALL_COIN) // 작은 코인일 경우
            {
                CoinManager.instance.GetCoin(SMALL_COIN_VALUE);
            }
            else if (mytype == CoinType.BIG_COIN) // 큰 코인일 경우
            {
                CoinManager.instance.GetCoin(BIG_COIN_VALUE);
            }

            // 사운드 추가            
            SoundManager.instance.PlaySound(GroupList.Item, (int)ItemSoundList.GetCoinSound);

            Destroy(this.gameObject);
        }
    }
}
