using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 상점 스킬 해금 클래스
/// 231218 배경택
/// </summary>
public class Store_SkillUnlock : StoreObject
{
    [Header("스킬 정보")]
    [SerializeField] private string skillName;
    [SerializeField] private int cost;
    [SerializeField] private string skillExplain;

    // 각각 자식 오브젝트의 인덱스 번호
    private const int NAME_INDEX = 1;
    private const int COST_INDEX = 2;
    private const int EXPLAIN_INDEX = 3;

    private TMP_Text nameObject;
    private TMP_Text costObject;
    private TMP_Text explainObject;

    [SerializeField] private GunMode gunMode;

    

    protected override void Awake()
    {
        base.Awake();
        // 스킬 정보 입력할 게임 오브젝트 캐싱
        //nameObject = transform.GetChild(NAME_INDEX).GetComponent<TMP_Text>();
        //costObject = transform.GetChild(COST_INDEX).GetComponent<TMP_Text>();
        //explainObject = transform.GetChild(EXPLAIN_INDEX).GetComponent<TMP_Text>();

        nameObject = Utility.FindChildObj(this.gameObject, "Text(Name)").GetComponent<TMP_Text>();
        costObject = Utility.FindChildObj(this.gameObject, "Text(Cost)").GetComponent<TMP_Text>();
        explainObject = Utility.FindChildObj(this.gameObject, "Text(Explain)").GetComponent<TMP_Text>();

        // 스킬 정보 입력
        nameObject.text = skillName;
        costObject.text = cost.ToString() + "개";
        explainObject.text = skillExplain;

        // 금액에 코스트 반영
        price = cost;
    }

    // 스킬 구매시 호출되는 함수
    public override void BuyStoreObject()
    {
        if (IsCanBuy() == false) return;
        base.BuyStoreObject();


        GameEventsManager.instance.coinEvents.UseCoin(gunMode);        
    }
}
