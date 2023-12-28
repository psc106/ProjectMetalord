using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class InteractUI : MonoBehaviour
{
   
    //private TMP_Text interactText = default;
    private readonly float fadeTime = 1f;
    public Image fadeImage = default;
    public Image idleImage = default;

    public bool isShowInteractUI = false;

    bool isFadeIn = false;
    bool isFadeOut = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOutImage(idleImage));
            StartCoroutine(FadeInImage(fadeImage));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInImage(idleImage));
            StartCoroutine(FadeOutImage(fadeImage));
        }
    }

   
    private IEnumerator FadeInImage(Image changeImage)
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

            Color color = changeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            changeImage.color = color;

            yield return null;
        }
        isFadeIn = false;
    }
    private IEnumerator FadeOutImage(Image changeImage)
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

            Color color = changeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            changeImage.color = color;

            yield return null;
        }
        isFadeOut = false;
    }

}
