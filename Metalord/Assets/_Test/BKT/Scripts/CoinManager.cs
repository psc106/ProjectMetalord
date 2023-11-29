using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoinType
{
    SMALL_COIN,
    BIG_COIN
}

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int startCoin = 10; // 시작 코인
    [SerializeField] private const int SMALL_COIN = 5; // 작은코인 값
    [SerializeField] private const int BIG_COIN = 100; // 큰 코인 값

    public int currentCoin { get; private set; }

    private void Awake()
    {
        currentCoin = startCoin; // 시작시 코인 세팅
    }

    private void OnEnable()
    {
        GameEventsManager.instance.coinEvents.onUseCoin += UseCoin;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.coinEvents.onUseCoin -= UseCoin;
    }

    private void Start()
    {
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 시작시 코인 변경 알림
    }

    public void GetCoin(int coin)
    {
        currentCoin += coin; // 코인을 현재 코인에 반영
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 먹을 경우 코인 변경
    }

    public void UseCoin(int coin)
    {
        currentCoin -= coin; // 사용한 코인 현재 코인에 반영
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 사용시 코인 변경
    }
}
