using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


/// <summary>
/// 환경설정 매니저
/// 231211_배경택
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("Grapics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;

    private bool isFullScreen;
    private float brightnessLevel;

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPrompt = null;

    private void Start()
    {
        //InitResolutionOption(); //시작시 해상도 옵션 초기화
        
    }

    //해상도 옵션 초기화
    void InitResolutionOption()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; // 옵션에 입력
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // 해상도 셋팅 
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
    }

    // 볼륨 셋팅
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    // 볼륨 적용
    public void VolumeApply()
    {
        //TODO 볼륨값 저장
        StartCoroutine(ConfirmationBox()); // 로딩 박스
    }

    // 전체화면 여부 셋팅
    public void SetFullScreen(bool _isFullscreen)
    {
        isFullScreen = _isFullscreen;
    }

    // 밝기 셋팅
    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    // 그래픽 설정 반영
    public void GraphicsApply()
    {
        //TODO 그래픽 설정 저장

        Screen.fullScreen = isFullScreen;

        StartCoroutine(ConfirmationBox()); // 로딩 박스
    }

    // 리셋 버튼 누를시
    public void ResetButton()
    {
        // 오디오 리셋
        AudioListener.volume = defaultVolume;
        volumeSlider.value = defaultVolume;
        volumeTextValue.text = defaultVolume.ToString("0.0");
        VolumeApply(); // 볼륨 저장

        // 밝기 리셋
        brightnessSlider.value = defaultBrightness;
        brightnessTextValue.text = defaultBrightness.ToString("0.0");

        // 전체화면 모드
        Screen.fullScreen = true;

        // 해상도 리셋
        Resolution currentResolution = Screen.currentResolution;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
        resolutionDropdown.value = resolutions.Length; // 내컴퓨터 최고 해상도가 마지막 번호로 들어가기 때문에 Length값으로 설정
        GraphicsApply(); // 그래픽 저장

    }

    // 적용 시간동안 표시해줄 박스_실제로 반영되는시간을 적용한것이 아님
    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        comfirmationPrompt.SetActive(false);
    }
}
