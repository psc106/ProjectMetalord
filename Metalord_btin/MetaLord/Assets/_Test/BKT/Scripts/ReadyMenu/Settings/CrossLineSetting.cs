using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossLineSetting : MonoBehaviour
{
    [SerializeField] GameObject crossLine; //경로 Player >> CameraSystem >> GroundCamera >> Canvas >> adjustedReticle

    private ButtonSize selectedButton;

    private void Start()
    {
        //TODO 타이틀씬 로드시 재작성
        if (PlayerPrefs.HasKey("CrossLineSetting"))
        {
            LoadData();
        }
        else
        {
            Init();
        }
    }

    private void OnDisable()
    {
        //TODO 타이틀씬 로드시 재작성
        SaveData();
    }

    // 버튼 누를시 호출되는 함수
    private void ChangeCrossLineSize(ButtonSize inputButton)
    {
        switch (inputButton)
        {
            case ButtonSize.Small:
                crossLine.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;

            case ButtonSize.Middle:
                crossLine.transform.localScale = new Vector3(1f, 1f, 1f);
                break;

            case ButtonSize.Large:
                crossLine.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
        }
    }

    // (소) 버튼 클릭시
    public void PushSmall()
    {
        selectedButton = ButtonSize.Small;
        ChangeCrossLineSize(selectedButton);
    }

    // (중) 버튼 클릭시
    public void PushMiddle()
    {
        selectedButton = ButtonSize.Middle;
        ChangeCrossLineSize(selectedButton);
    }

    // (대) 버튼 클릭시
    public void PushLarge()
    {
        selectedButton = ButtonSize.Large;
        ChangeCrossLineSize(selectedButton);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("CrossLineSetting", (int)selectedButton);
    }

    private void LoadData()
    {
        selectedButton = (ButtonSize)PlayerPrefs.GetInt("CrossLineSetting");
        ChangeCrossLineSize(selectedButton);
    }

    public void Init()
    {
        PushMiddle();
    }
}
