using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RecordObject : MonoBehaviour,IPointerDownHandler
{
    public RecordObjectInfo recordInfo { get; set; }
   
    private void OnEnable()
    {
        GameEventsManager.instance.recordEvents.onChangeRecord += ReflectInfo; // 도감 변경알림시 갖고있는 정보 반영
    }

    private void OnDisable()
    {
        GameEventsManager.instance.recordEvents.onChangeRecord -= ReflectInfo; // 도감 변경알림시 갖고있는 정보 반영
    }

    private void Start()
    {
        ReflectInfo(); // 이미지 초기화
    }

    private void ReflectInfo()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Object/" + recordInfo.id_Description);
    }

    /// <summary>
    /// 오브젝트가 마우스로 클릭되었을때 실행되는 함수
    /// 231204 배경택
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerDown(PointerEventData eventData)
    {
        RecordManager.instance.InputRecordInfo(recordInfo);
    }

    

    
}
