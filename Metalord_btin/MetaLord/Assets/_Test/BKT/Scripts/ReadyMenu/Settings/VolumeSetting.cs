using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

/// <summary>
/// 볼륨 셋팅하는 클래스
/// 240103 배경택
/// </summary>
public class VolumeSetting : MonoBehaviour
{
    [Header("오디오 믹서")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("오디오 슬라이더")]
    [SerializeField] private Slider slider_Master;
    [SerializeField] private Slider slider_BGM;
    [SerializeField] private Slider slider_SFX;

    [Header("오디오 토글 스위치")]
    [SerializeField] private Toggle toggle_Master;
    [SerializeField] private Toggle toggle_BGM;
    [SerializeField] private Toggle toggle_SFX;

    [Header("오디오 변경 관련 스프라이트")]
    [SerializeField] private Sprite sprite_Toggle_True;
    [SerializeField] private Sprite sprite_Toggle_False;
    [SerializeField] private Sprite sprite_SliderBar_True;
    [SerializeField] private Sprite sprite_SliderBar_False;
    [SerializeField] private Sprite sprite_Pointer_True;
    [SerializeField] private Sprite sprite_Pointer_False;

    [Header("오디오 음량(%) 텍스트")]
    [SerializeField] private TMP_Text text_Master;
    [SerializeField] private TMP_Text text_BGM;
    [SerializeField] private TMP_Text text_SFX;


    private float vol_Master;
    private float vol_BGM;
    private float vol_SFX;

    private void Awake()
    {
        // 슬라이더 함수 입력
        slider_Master.onValueChanged.AddListener(SetMasterVolume);
        slider_BGM.onValueChanged.AddListener(SetBGMVolume);
        slider_SFX.onValueChanged.AddListener(SetSFXVolume);

        // 토글 함수 입력
        toggle_Master.onValueChanged.AddListener(SetControlMaster);
        toggle_BGM.onValueChanged.AddListener(SetControlBGM);
        toggle_SFX.onValueChanged.AddListener(SetControlSFX);
    }

    // 마스터 볼륨 설정
    public void SetMasterVolume(float vol)
    {
        vol_Master = vol;
        AudioMixerSet("Master", vol);
        //audioMixer.SetFloat("Master",Mathf.Log10(vol)*20);
        text_Master.text = ((int)(vol * 100)).ToString() + "%";
    }

    // BGM 볼륨 설정
    public void SetBGMVolume(float vol)
    {
        vol_BGM = vol;
        AudioMixerSet("BGM", vol);

        //audioMixer.SetFloat("BGM", Mathf.Log10(vol)*20);
        text_BGM.text = ((int)(vol * 100)).ToString() + "%";
    }

    // SFX 볼륨 설정
    public void SetSFXVolume(float vol)
    {
        vol_SFX = vol;
        AudioMixerSet("SFX", vol);
        //audioMixer.SetFloat("SFX", Mathf.Log10(vol)*20);
        text_SFX.text = ((int)(vol * 100)).ToString() + "%";
    }

    // 마스터 조절 여부 설정
    public void SetControlMaster(bool check)
    {
        GameObject tempToggle = toggle_Master.gameObject;
        GameObject tempSlider = slider_Master.gameObject;

        if (check == false)
        {
            AudioMixerSet("Master", 0f);
            SetFalse(tempToggle, tempSlider);
            // BGM,SFX 함께 조절
            toggle_BGM.onValueChanged.Invoke(false);
            toggle_SFX.onValueChanged.Invoke(false);
            toggle_BGM.interactable = false;
            toggle_SFX.interactable = false;

        }
        else
        {
            SetTrue(tempToggle, tempSlider);
            AudioMixerSet("Master", vol_Master);
            // BGM, SFX 함께 조절
            toggle_BGM.onValueChanged.Invoke(true);
            toggle_SFX.onValueChanged.Invoke(true);
            toggle_BGM.interactable = true;
            toggle_SFX.interactable = true;
        }
    }

    // BGM 조절 여부 설정
    public void SetControlBGM(bool check)
    {        
        GameObject tempToggle = toggle_BGM.gameObject;
        GameObject tempSlider = slider_BGM.gameObject;

        if (check == false)
        {
            AudioMixerSet("BGM", 0f);
            SetFalse(tempToggle, tempSlider);

        }
        else
        {
            SetTrue(tempToggle, tempSlider);
            AudioMixerSet("BGM", vol_BGM);

        }
    }

    // SFX 조절 여부 설정
    public void SetControlSFX(bool check)
    {
        GameObject tempToggle = toggle_SFX.gameObject;
        GameObject tempSlider = slider_SFX.gameObject;

        if (check == false)
        {
            AudioMixerSet("SFX", 0f);
            SetFalse(tempToggle, tempSlider);

        }
        else
        {
            SetTrue(tempToggle, tempSlider);
            AudioMixerSet("SFX", vol_SFX);

        }
    }

    // 조절 False로 설정시 변경내용
    private void SetFalse(GameObject tempToggle, GameObject tempSlider)
    {
        tempSlider.GetComponent<Slider>().interactable = false;
        tempSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprite_SliderBar_False;
        tempSlider.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = sprite_Pointer_False;

        tempToggle.GetComponent<Image>().sprite = sprite_Toggle_False;
        tempToggle.transform.GetChild(0).gameObject.SetActive(true);
        tempToggle.transform.GetChild(1).gameObject.SetActive(false);
        tempToggle.transform.GetChild(2).gameObject.SetActive(true);
        tempToggle.transform.GetChild(3).gameObject.SetActive(false);
    }

    // 조절 True로 설정시 변경내용
    private void SetTrue(GameObject tempToggle, GameObject tempSlider)
    {
        tempSlider.GetComponent<Slider>().interactable = true;
        tempSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprite_SliderBar_True;
        tempSlider.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = sprite_Pointer_True;

        tempToggle.GetComponent<Image>().sprite = sprite_Toggle_True;
        tempToggle.transform.GetChild(0).gameObject.SetActive(false);
        tempToggle.transform.GetChild(1).gameObject.SetActive(true);
        tempToggle.transform.GetChild(2).gameObject.SetActive(false);
        tempToggle.transform.GetChild(3).gameObject.SetActive(true);
    }

    private void AudioMixerSet(string id, float vol)
    {        
        audioMixer.SetFloat(id, Mathf.Log10(vol) * 20);
    }


    private void SaveData()
    {

    }

    private void LoadData()
    {

    }
}
