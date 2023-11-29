using System;
using UnityEngine;

/// <summary>
/// 도감 이벤트
/// </summary>
public class RecordEvents
{
    public RecordEvents()
    {
        Debug.Log("RecordEvents 생성");
    }

    public event Action<string> onGetRecordItem;
    public void GetRecordItem(string id)
    {
        if(onGetRecordItem != null) // 이벤트함수가 비어있지 않다면
        {
            onGetRecordItem(id);
        } 
    }
}
