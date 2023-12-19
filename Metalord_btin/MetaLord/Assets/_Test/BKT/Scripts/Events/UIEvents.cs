using System;
using UnityEngine;

/// <summary>
/// 도감 이벤트
/// </summary>
public class UIEvents
{
    public UIEvents()
    {
        Debug.Log("UI Events 생성");
    }

    /// <summary>
    /// UI 버튼을 누를경우
    /// 231130 배경택
    /// </summary>
    public event Action onUIOpenButton;
    public void PushUIButton()
    {
        if(onUIOpenButton != null) // 이벤트함수가 비어있지 않다면
        {
            onUIOpenButton();
        } 
    }
}
