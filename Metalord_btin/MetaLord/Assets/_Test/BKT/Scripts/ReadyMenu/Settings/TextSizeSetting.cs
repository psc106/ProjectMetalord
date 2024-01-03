using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextSizeSetting : MonoBehaviour
{
    private ButtonSize selectedButton;

    TMP_Text[] originTexts; // 크기를 기억할 원본
    List<float> fontSizes;    
    //TMP_Text[] copyTexts; // 크기를 기억할 원본


    private void Awake()
    {
        // 비활성화된 오브젝트도 전부 가져와서 배열로 담음
        originTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();    
        
        fontSizes = new List<float>();

        // 각각 크기가 다른 텍스트의 원래 크기값을 리스트에 저장해줌
        for (int i = 0; i < originTexts.Length; i++)
        {            
            fontSizes.Add(originTexts[i].fontSize);
            //Debug.Log(originTexts[i].name);
        }

        //Debug.Log(originTexts.Length);
    }

    private void Start()
    {
        //TODO 타이틀씬 로드시 재작성
        //if (PlayerPrefs.HasKey("TextSizeSetting"))
        //{
        //    LoadData();
        //}
        //else
        {
            selectedButton = ButtonSize.Middle;

        }
    }

    private void OnDisable()
    {
        //TODO 타이틀씬 로드시 재작성
        //SaveData();
    }

    // 버튼 누를시 호출되는 함수
    private void ChangeTextSize(ButtonSize inputButton)
    {
        float tempSize = default;

        for(int i = 0; i < originTexts.Length; i++)
        {
            switch (inputButton)
            {
                case ButtonSize.Small:
                    tempSize = fontSizes[i] * 0.8f;
                    break;

                case ButtonSize.Middle:
                    tempSize = fontSizes[i];
                    break;

                case ButtonSize.Large:
                    tempSize = fontSizes[i]* 1.2f;
                    break;
            }
            originTexts[i].fontSize = tempSize;
        }
    }

    // (소) 버튼 클릭시
    public void PushSmall()
    {
        selectedButton = ButtonSize.Small;
        ChangeTextSize(selectedButton);
    }

    // (중) 버튼 클릭시
    public void PushMiddle()
    {
        selectedButton = ButtonSize.Middle;
        ChangeTextSize(selectedButton);
    }

    // (대) 버튼 클릭시
    public void PushLarge()
    {
        selectedButton = ButtonSize.Large;
        ChangeTextSize(selectedButton);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("TextSizeSetting", (int)selectedButton);
    }

    private void LoadData()
    {
        selectedButton = (ButtonSize)PlayerPrefs.GetInt("TextSizeSetting");
        ChangeTextSize(selectedButton);
    }
}
