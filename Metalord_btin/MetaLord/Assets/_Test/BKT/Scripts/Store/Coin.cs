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

    [Header("작은코인 0~49, 큰 코인 50~99")]
    [SerializeField] private int id; // 겹치지 않도록 에디터에서 id 입력

    const int FALSE = 0;
    const int TRUE = 1;
    private int isExist = TRUE; // 존재하는가

    // 코인 증가량
    private const int SMALL_COIN_VALUE = 5; // 작은코인 값
    private const int BIG_COIN_VALUE = 20; // 큰 코인 값

    private void Start()
    {
        GameEventsManager.instance.dataEvents.onSaveData += SaveData;
        GameEventsManager.instance.dataEvents.onLoadData += LoadData;
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.dataEvents.onSaveData -= SaveData;
        GameEventsManager.instance.dataEvents.onLoadData -= LoadData;
    }

    // 코인 활성화 여부 저장
    private void SaveData()
    {
        DataManager.instance.savedGamePlayData.coinAndRecordItem[id] = isExist;
    }

    // 코인 활성화 여부 불러오기
    private void LoadData()
    {
        isExist = DataManager.instance.savedGamePlayData.coinAndRecordItem[id];
        CheckIsExist();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
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
            isExist = FALSE;
            gameObject.SetActive(false); //비활성화로 변경
        }
    }

    // 코인이 존재하는지 체크
    private void CheckIsExist()
    {
        if(isExist == FALSE)
        {
            isExist = FALSE;
            gameObject.SetActive(false); //비활성화로 변경
        }
    }
}
