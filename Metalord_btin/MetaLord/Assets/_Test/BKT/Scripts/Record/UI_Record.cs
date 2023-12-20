using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// 도감 UI 클래스
/// 231220 배경택
/// </summary>
public class UI_Record : MonoBehaviour
{
    [SerializeField] private GameObject labelSortGot;
    [SerializeField] private GameObject labelSortZone;

    private void Start()
    {
        ResetSort();
    }

    // 켜질때 정렬 초기화
    private void OnEnable()
    {

        ResetSort();

    }

    private void ResetSort()
    {
        RecordManager.instance.SortGot(0);
        RecordManager.instance.SortZone(0);

        labelSortGot.GetComponent<TMP_Text>().text = "지역";
        labelSortZone.GetComponent<TMP_Text>().text = "획득 여부";
        
        //Utility.FindChildObj(this.gameObject, "Label(SortGot)").GetComponent<TMP_Text>().text = "지역";
        //Utility.FindChildObj(this.gameObject, "Label(SortZone)").GetComponent<TMP_Text>().text = "획득 여부";
    }

}
