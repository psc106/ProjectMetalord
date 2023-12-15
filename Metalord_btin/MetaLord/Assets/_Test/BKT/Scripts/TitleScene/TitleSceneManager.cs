using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    [Header("Title Display")]
    [SerializeField] private GameObject anykeyDisplay; //최초 아무키 누르면 사라지는 디스플레이
    [SerializeField] private GameObject titleDisplay; // 타이틀 화면
    [SerializeField] private GameObject gameStartDisplay; // 게임 시작을 위한 화면
    [SerializeField] private GameObject settingDisplay; // 환경 설정을 위한 화면
    [SerializeField] private GameObject explainDisplay; // 게임 설명 화면

    [Header("Volume Setting")]
    [SerializeField] private TextMeshProUGUI volumeTextValue = null;
    [SerializeField] private SliderJoint2D volumeSlider = null;
    [SerializeField] private GameObject comfirmationPrompt = null;

    private bool isPushAnyKey = false;

    private void Update()
    {
        if (Input.anyKeyDown && !isPushAnyKey) //아무키나 누르면 타이틀 화면이 나옴
        {
            isPushAnyKey = true;
            anykeyDisplay.SetActive(false);
            titleDisplay.SetActive(true);
        }
    }

    // 나가기 버튼 게임 종료
    public void onExitButton()
    {
        Application.Quit();
    }

    //// 시작 버튼 누를 시
    //public void onStartButton()
    //{
    //    titleDisplay.SetActive(false);
    //    gameStartDisplay.SetActive(true);
    //}

    //// 환경설정 버튼 누를 시
    //public void onSettingsButton()
    //{
    //    titleDisplay.SetActive(false);
    //    settingDisplay.SetActive(true);
    //}

    //// 설명 버튼 누를 시
    //public void onExplainButton()
    //{
    //    titleDisplay.SetActive(false);
    //    explainDisplay.SetActive(true);

    //}

    //// 타이틀화면으로 돌아가기
    //public void backTitle()
    //{
    //    // 타이틀 화면 On
    //    titleDisplay.SetActive(true);

    //    // 나머지 화면 Off
    //    gameStartDisplay.SetActive(false);
    //    settingDisplay.SetActive(false);
    //    explainDisplay.SetActive(false);
    //}
}
