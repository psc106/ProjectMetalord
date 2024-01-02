using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 디스플레이 셋팅(전체화면, 창화면)
/// 240102 배경택
/// </summary>
public class DisplaySetting : MonoBehaviour
{
    private int isFullScreen;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("DisplaySetting")) LoadData(); // 저장되어있는 데이터가 있다면
        else isFullScreen = 1; // 없다면
        ChangeScreenMode();
    }

    private void OnDisable()
    {
        SaveData();
    }

    // 스크린 int값에 따라 스크린 모드 변경
    public void ChangeScreenMode()
    {
        if (isFullScreen == 1)
        {
            Screen.fullScreen = true;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Screen.fullScreen = false;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }    

    // 데이터 저장
    private void SaveData()
    {
        PlayerPrefs.SetInt("DisplaySetting", isFullScreen);
    }


    // 데이터 불러오기
    private void LoadData()
    {
       isFullScreen = PlayerPrefs.GetInt("DisplaySetting");
    }
}
