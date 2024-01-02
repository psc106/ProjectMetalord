using UnityEngine;

/// <summary>
/// 재화 매니저
/// 231129_배경택
/// </summary>
public class CoinManager : MonoBehaviour
{
    static public CoinManager instance;

    [Header("상점 UI")]
    [SerializeField] private GameObject storeUI;

    [Header("게임 시작 코인")]
    [SerializeField] private int startCoin = 10; // 시작 코인

    [Header("코인 설명 UI")]
    [SerializeField] private GameObject coinExplain;


    public int currentCoin = 0;
    private bool isFirst = true;

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
        isFirst = true;                                 
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dataEvents.onSaveData += SaveCoin;
        GameEventsManager.instance.dataEvents.onLoadData += LoadCoin;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dataEvents.onSaveData -= SaveCoin;
        GameEventsManager.instance.dataEvents.onLoadData -= LoadCoin;
    }

    // 현재 코인 저장
    private void SaveCoin()
    {
        DataManager.instance.savedGamePlayData.money = currentCoin;
    }

    // 코인 불러오기
    private void LoadCoin()
    {
        currentCoin = DataManager.instance.savedGamePlayData.money;
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 먹을 경우 코인 변경
    }

    private void Start()
    {
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 시작시 코인 변경 알림
    }

    public void GetCoin(int coin)
    {
        if (isFirst) // 첫 획득시 설명을 띄워줌
        {
            isFirst = false;
            Controller_Physics.SwitchCameraLock(false);
            coinExplain.SetActive(true);
        }
        currentCoin += coin; // 코인을 현재 코인에 반영
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 먹을 경우 코인 변경

    }

    public void UseCoin(int coin)
    {
        currentCoin -= coin; // 사용한 코인 현재 코인에 반영
        GameEventsManager.instance.coinEvents.ChangeCoin(currentCoin); // 코인 사용시 코인 변경
    }
}
