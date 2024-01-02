using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// 해상도 세팅
/// 240102_배경택
/// </summary>
public class ResolutionSetting : MonoBehaviour
{
    [SerializeField] private DisplaySetting displaySetting; // 화면모드를 가져올 변수

    private Resolution[] resolutions; // 해상도를 담을 변수
    private List<string> str_resolution = new List<string>();
    private TMP_Text resText; // 해상도 표현 텍스트
    private int resIndex; // 해상도를 가리킬 인덱스값

    private void Awake()
    {
        resText = transform.GetChild(0).GetComponent<TMP_Text>(); // 글자 표시 텍스트        

    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string temp = resolutions[i].width + "x" + resolutions[i].height;
            str_resolution.Add(temp);

            if(Screen.currentResolution.width == resolutions[i].width && Screen.currentResolution.height == resolutions[i].height)
            {
                resIndex = i;
            }
        }

        InputText(resIndex);
    }


    // 좌측버튼 누를경우
    public void PushLeft()
    {
        if (resIndex > 0)
        {
            resIndex--;

            InputText(resIndex);
            Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height,Convert.ToBoolean(displaySetting.isFullScreen));
        } 
    }

    // 우측버튼 누를경우
    public void PushRight()
    {
        if (resIndex < resolutions.Length)
        {
            resIndex++;

            InputText(resIndex);
            Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, Convert.ToBoolean(displaySetting.isFullScreen));
        }

    }

    // 텍스트 입력
    public void InputText(int _resIndex)
    {
        resText.text = str_resolution[_resIndex];
    }
}
