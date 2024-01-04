using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

/// <summary>
/// 화면 밝기 설정
/// 240103 배경택
/// </summary>
public class BrightnessSetting : MonoBehaviour
{
    private TMP_Text percentText;

    public PostProcessProfile brightness;
    private AutoExposure autoExposure;
    private Slider brightnessSlider;
    
    private float brightnessValue;

    private Image[] images;
    TMP_Text[] texts;

    private void Awake()
    {
        brightnessSlider = GetComponent<Slider>();
        percentText = transform.GetChild(3).GetComponent<TMP_Text>();                
        brightness.TryGetSettings(out autoExposure);
    }

    private void Start()
    {
        // TODO 타이틀씬 로드시 재작성
        //if(PlayerPrefs.HasKey("BrightnessSetting"))
        //{
        //    LoadData();
        //}
        //else
        {
            Init();
        }        
    }

    private void OnDisable()
    {
        // TODO 타이틀씬 로드시 재작성
        //SaveData();
    }

    // 밝기 조절 함수 _ 슬라이더로 조절
    public void ControllBrightness(float _value)
    {
        if(_value > 0.05f)
        {

            autoExposure.keyValue.value = _value;
            AdjustBrightness(_value);
        }
        else
        {
            autoExposure.keyValue.value = 0.05f;
            AdjustBrightness(0.05f);
        }
        
        percentText.text = ((int)(_value * 100)).ToString() + "%";

        brightnessValue = _value;
        //Debug.Log(_value);
    }


    private void Get2DImages()
    {
        images = Resources.FindObjectsOfTypeAll<Image>();
    }

    private void GetTexts()
    {
        texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
    }

    // 밝기 조절
    private void AdjustBrightness(float value)
    {
        float alpha = default;

        // UI 이미지의 밝기 조절 위한 RGB값 조절
        foreach (Image image in images)
        {
            if (image.transform.name == "Panel") continue;
            image.color = new Color(value, value, value, image.color.a);
        }

        // 텍스트 밝기 조절을 위한 알파값 조절
        foreach(TMP_Text text in texts)
        {
            if (text.color.r < 0.5 && value < 0.7) alpha = 0.7f; // 검은색글씨처럼 어두운 글씨는 알파값을 0.7까지 조절
            else alpha = value; // 흰색과같은 밝은 글씨는 알파값을 0.05까지 조절
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
    }

    // 데이터 저장
    private void SaveData()
    {
        PlayerPrefs.SetFloat("BrightnessSetting", brightnessValue);        
    }


    // 데이터 불러오기
    private void LoadData()
    {
        brightnessValue = PlayerPrefs.GetInt("BrightnessSetting");
        ControllBrightness(brightnessValue);
        brightnessSlider.value = brightnessValue;
    }

    public void Init()
    {
        brightnessValue = 1f;
        brightnessSlider.value = brightnessValue;
        Get2DImages();
        GetTexts();
        ControllBrightness(brightnessValue);
    }
}
