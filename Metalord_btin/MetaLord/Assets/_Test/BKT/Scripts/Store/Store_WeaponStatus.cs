using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 상점 무기 업그레이드 클래스
/// 231218 배경택
/// </summary>
public class Store_WeaponStatus : StoreObject
{
    [Header("스킬 정보")]
    [SerializeField] private string skillName;
    [SerializeField] private UpgradeCategory upgradeCategory;

    [Header("업그레이드 이미지")]
    [SerializeField] private Sprite stackOn;    

    private TMP_Text nameObject;
    private TMP_Text costObject;

    private GameObject coinImage;
    private GameObject soldOutText;


    private  const int MAX_STEP = 13; // 최대 단계
    private int stepIndex; // 스텝을 가리키는 index

    [Header("업그레이드 단계별 정보")]
    [SerializeField] int[] stepCost = new int[MAX_STEP]; // 단계별 소모 코인 양
    [SerializeField] int[] stepIncreaseAmount = new int[MAX_STEP]; // 단계별 증가량


    protected override void Awake()
    {
        base.Awake();

        stepIndex = 0;

        // 스킬 정보 입력할 게임 오브젝트 캐싱
        //nameObject = transform.GetChild(NAME_INDEX).GetComponent<TMP_Text>();
        //costObject = transform.GetChild(COST_INDEX).GetChild(COST_INDEX).GetComponent<TMP_Text>();

        nameObject = Utility.FindChildObj(this.gameObject, "Text(Name)").GetComponent<TMP_Text>();
        costObject = Utility.FindChildObj(this.gameObject, "Text(Cost)").GetComponent<TMP_Text>();
        coinImage = Utility.FindChildObj(this.gameObject, "Coin_Image");
        soldOutText = Utility.FindChildObj(this.gameObject, "Text(SoldOut)");

        // 스킬 정보 입력
        nameObject.text = skillName;
        costObject.text = stepCost[stepIndex].ToString() + "개";
        price = stepCost[stepIndex];
    }

    protected override void SaveData()
    {
        base.SaveData();        
        DataManager.instance.savedGamePlayData.upgrade[(int)upgradeCategory] = stepIndex;
    }

    protected override void LoadData()
    {
        stepIndex = DataManager.instance.savedGamePlayData.upgrade[(int)upgradeCategory];
        ReflectLoadUpgradeInfo();
        base.LoadData();        
    }

    // 불러온 업그레이드 정보 적용
    private void ReflectLoadUpgradeInfo()
    {
        for(int i = 0; i < stepIndex; i++) // 스텝 인덱스까지 이미지 적용
        {
            ReflectStepImage(i);
        }
        ReflectCostText(stepIndex); // 금액 적용
        ReflectCost(stepIndex); // 금액 적용
        if (stepIndex == MAX_STEP) isCanBuy = false; // 만약 최고 스텝이라면 구매 불가
        ChangeButtonUI(0); // UI 적용
    }

    // 웨폰 업그레이드 구매시
    protected override void BuyStoreObject()
    {
        if (IsCanBuy() == false)
        {
            PlayCantBuySound();
            return;
        }
        else PlayCanBuySound();


        // 최대 스텝까지만 실행
        if (stepIndex >= MAX_STEP)
        {
            isCanBuy = false;
        }
            base.BuyStoreObject();
            ReflectStepImage(stepIndex);
            ReflectCostText(stepIndex);
            ReflectCost(stepIndex);
            ChangeButtonUI(0);

            GameEventsManager.instance.coinEvents.UpgradeGun(upgradeCategory,stepIncreaseAmount[stepIndex]); // 건 업그레이드에 전달
            stepIndex += 1;
    }

    // 업그레이드 이미지 채워지는거 반영
    private void ReflectStepImage(int _index)
    {
        Image stepImage = Utility.FindChildObj(this.gameObject,"Steps").transform.GetChild(_index).GetComponent<Image>();
        stepImage.sprite = stackOn;
    }

    // 업그레이드 텍스트 반영
    private void ReflectCostText(int _index)
    {
        if (stepIndex < MAX_STEP - 1) costObject.text = stepCost[stepIndex + 1].ToString() + "개"; // 다음 스텝의 금액을 반영
        else
        {
            coinImage.SetActive(false);
            soldOutText.SetActive(true);
            costObject.enabled = false;
        }
    }

    // 업그레이드 텍스트 반영
    private void ReflectCost(int _index)
    {
        if (stepIndex < MAX_STEP - 1) price = stepCost[stepIndex + 1]; // 다음 스텝의 금액을 반영
        else price = 9999999; // 구매 불가
    }
}
