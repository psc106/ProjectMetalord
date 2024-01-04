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
        }
        else
        {
            autoExposure.keyValue.value = 0.05f;
        }
        
        percentText.text = ((int)(_value * 100)).ToString() + "%";

        brightnessValue = _value;
        //Debug.Log(_value);
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
        ControllBrightness(brightnessValue);
    }
}
