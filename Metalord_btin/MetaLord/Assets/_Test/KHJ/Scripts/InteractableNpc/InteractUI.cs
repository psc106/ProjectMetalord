using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class InteractUI : MonoBehaviour
{
    public GameObject interactPanel = default;
    //private TMP_Text interactText = default;
    public float fadeTime = 0f;
    public Image fadeImage = default;


    bool isFadeIn = false;
    bool isFadeOut = false;
    
    private void Start()
    {
        interactPanel.SetActive(false);
    }

   
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(FadeInImage());
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("나갔는데 실행됨?");
        StartCoroutine(FadeOutImage());
    }

   
    private IEnumerator FadeInImage()
    {
        if (isFadeIn)
        {
            yield break;
        }
        float current = 0;
        float percent = 0;
        float start = 0f;
        float end = 1f;
        while (percent < 1)
        {
            isFadeIn = true;
            
            current += 2f * Time.deltaTime;
            percent = current / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }
        isFadeIn = false;
    }
    private IEnumerator FadeOutImage()
    {
        if (isFadeOut)
        {
            yield break; // 이미 다른 FadeOutImage 코루틴이 실행 중이면 종료
        }
        float current = 0;
        float percent = 0;
        float start = 1f;
        float end = 0f;
       
        while (percent < 1 )
        {
            
            isFadeOut = true;

            current += 2f * Time.deltaTime;
            percent = current / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }
        isFadeOut = false;
    }

}
