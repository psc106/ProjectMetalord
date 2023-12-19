using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RecordObject : MonoBehaviour,IPointerDownHandler
{
    public RecordObjectInfo recordInfo { get; set; }

    private void Awake()
    {
        GameEventsManager.instance.recordEvents.onSelectRecord += CheckSelected; // 선택여부 체크 추가
        GameEventsManager.instance.recordEvents.onChangeRecord += ReflectInfo; // 도감 변경알림시 갖고있는 정보 반영 추가
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.recordEvents.onChangeRecord -= ReflectInfo; // 도감 변경알림시 갖고있는 정보 반영 해제
        GameEventsManager.instance.recordEvents.onSelectRecord -= CheckSelected; // 선택여부 체크 해제
    }


    private void Start()
    {
        ReflectInfo(); // 초기화
    }

    /// <summary>
    /// 수집 아이템 정보를 반영하는 함수
    /// </summary>
    private void ReflectInfo()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Object/" + recordInfo.id_Description);

        if(recordInfo.obtained == true) transform.GetChild(0).gameObject.SetActive(false);
        else transform.GetChild(0).gameObject.SetActive(true);

        if (recordInfo.isSelected == true) transform.GetChild(1).gameObject.SetActive(true);
        else transform.GetChild(1).gameObject.SetActive(false);
    }

    /// <summary>
    /// 도감 아이템의 선택여부를 체크하는 함수
    /// 231206 배경택
    /// </summary>
    /// <param name="_id"></param>
    private void CheckSelected(int _id)
    {
        if (recordInfo.id != _id)
        {
            InActiveChecking();
        }
        else // 선택된 id값과 내 id값이 같고
        {
            if (!recordInfo.isSelected) // 선택된 적이 없다면
            {
                ActiveChecking();
            }
            else // 선택된 적이 있다면
            {
                InActiveChecking();
            }
        }
    }

    /// <summary>
    /// 체크표시 비활성화
    /// </summary>
    private void InActiveChecking()
    {
        recordInfo.isSelected = false;
        transform.GetChild(1).gameObject.SetActive(false);
    }

    /// <summary>
    /// 체크표시 활성화
    /// </summary>
    private void ActiveChecking()
    {
        recordInfo.isSelected = true; //선택되었다고 표시
        transform.GetChild(1).gameObject.SetActive(true); //선택되었다고 표시
    }

    /// <summary>
    /// 오브젝트가 마우스로 클릭되었을때 실행되는 함수
    /// 231204 배경택
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerDown(PointerEventData eventData)
    {
        GameEventsManager.instance.recordEvents.SelectRecord(recordInfo.id);
    }
}
