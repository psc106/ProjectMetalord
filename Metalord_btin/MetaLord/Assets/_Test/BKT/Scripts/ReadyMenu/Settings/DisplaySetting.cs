using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 디스플레이 셋팅(전체화면, 창화면)
/// 240102 배경택
/// </summary>
public class DisplaySetting : MonoBehaviour
{
    private Image[] images;

    private int _isFullScreen;
    public int isFullScreen
    {
        get { return _isFullScreen; }
    }

    private void Get2DImage()
    {
        images = Resources.FindObjectsOfTypeAll<Image>();
    }

    private void Awake()
    {
        // TODO 타이틀씬 로드시 재작성
        //if (PlayerPrefs.HasKey("DisplaySetting"))
        //{
        //    LoadData(); // 저장되어있는 데이터가 있다면
        //    ChangeScreenMode();            
        //}
        //else
            Init(); // 없다면
    }

    private void OnDisable()
    {
        //SaveData();
    }

    // 스크린 int값에 따라 스크린 모드 변경
    public void ChangeScreenMode()
    {
        //Debug.Log("버튼 클릭 됨");

        if (_isFullScreen == 1)
        {
            _isFullScreen = 0;

            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            Screen.fullScreen = true;

        }
        else
        {
            _isFullScreen = 1;

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            Screen.fullScreen = false;

        }
    }    

    // 데이터 저장
    private void SaveData()
    {
        PlayerPrefs.SetInt("DisplaySetting", _isFullScreen);
    }


    // 데이터 불러오기
    private void LoadData()
    {
       _isFullScreen = PlayerPrefs.GetInt("DisplaySetting");
    }

    // 초기화
    public void Init()
    {
        _isFullScreen = 1;
        ChangeScreenMode();        
    }
}
