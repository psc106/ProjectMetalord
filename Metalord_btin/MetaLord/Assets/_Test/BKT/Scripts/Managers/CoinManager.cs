using UnityEngine;

public enum CoinType
{
    SMALL_COIN,
    BIG_COIN
}

/// <summary>
/// 재화 매니저
/// 231129_배경택
/// </summary>
public class CoinManager : MonoBehaviour
{
    static public CoinManager instance;

    [SerializeField] private int startCoin = 10; // 시작 코인

    public int currentCoin { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        currentCoin = startCoin; // 시작시 코인 세팅
    }

    private void OnEnable()
    {
        Debug.Log(GameEventsManager.instance);
        //GameEventsManager.instance.coinEvents.onUseCoin += UseCoin;
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 먹을 경우 코인 변경
    }

    private void OnDisable()
    {
        //GameEventsManager.instance.coinEvents.onUseCoin -= UseCoin;
    }

    private void Start()
    {
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 시작시 코인 변경 알림
    }

    public void GetCoin(int coin)
    {
        currentCoin += coin; // 코인을 현재 코인에 반영
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 먹을 경우 코인 변경

        Debug.Log(currentCoin);
    }

    public void UseCoin(int coin)
    {
        currentCoin -= coin; // 사용한 코인 현재 코인에 반영
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 사용시 코인 변경
    }
}
