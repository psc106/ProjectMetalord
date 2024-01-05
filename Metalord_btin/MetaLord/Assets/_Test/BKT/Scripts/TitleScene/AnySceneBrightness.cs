using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnySceneBrightness : MonoBehaviour
{
    private Image[] images;
    private TMP_Text[] texts;

    private void Awake()
    {
        Get2DImages();
        GetTexts();
    }

    private void Start()
    {
        // 시작시 밝기 조절
        ControllBrightness(StartInfo.instance.info_Brightness);
    }

    private void Get2DImages()
    {
        images = Resources.FindObjectsOfTypeAll<Image>();
    }

    private void GetTexts()
    {
        texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
    }

    // 밝기 조절 함수 _ 슬라이더로 조절
    public void ControllBrightness(float _value)
    {
        if (_value > 0.05f)
        {           
            AdjustBrightness(_value);
        }
        else
        {            
            AdjustBrightness(0.05f);
        }              
    }

    // 밝기 조절
    private void AdjustBrightness(float value)
    {
        float alpha = value;
        float imgValue = value;

        // UI 이미지의 밝기 조절 위한 RGB값 조절
        foreach (Image image in images)
        {
            if (image.transform.name == "Panel") continue;
            if (imgValue < 0.5f) imgValue = 0.5f;
            image.color = new Color(imgValue, imgValue, imgValue, image.color.a);
        }

        // 텍스트 밝기 조절을 위한 알파값 조절
        foreach (TMP_Text text in texts)
        {
            if (text.color.r < 0.5f && alpha < 0.7f) alpha = 0.7f; // 검은색글씨처럼 어두운 글씨는 알파값을 0.7까지 조절
            else if (alpha < 0.1f) alpha = 0.1f; // 흰색과같은 밝은 글씨는 알파값을 0.1f까지 조절

            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
    }
}
