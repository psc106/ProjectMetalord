using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiFadeOut : MonoBehaviour
{
    float fadeOutspeed = 0.75f;

    TextMeshProUGUI[] myText;
    Image[] myImage;

    Color[] imageOrigin;
    Color[] textOrigin;

    IEnumerator fadeOut;

    private void Awake()
    {
        myText = GetComponentsInChildren<TextMeshProUGUI>();
        myImage = GetComponentsInChildren<Image>();

        imageOrigin = new Color[myImage.Length];
        textOrigin = new Color[myText.Length];

        for (int i = 0; i < myImage.Length; i++)
        {
            imageOrigin[i] = myImage[i].color;
        }

        for (int i = 0; i < myText.Length; i++)
        {
            textOrigin[i] = myText[i].color;
        }
    }

    private void Start()
    {
        fadeOut = FadeOut();
        InitFadeOut();
    }

    //private void OnEnable()
    //{
    //    for (int i = 0; i < myImage.Length; i++)
    //    {
    //        myImage[i].color = imageOrigin[i];
    //    }

    //    for (int i = 0; i < myText.Length; i++)
    //    {
    //        myText[i].color = textOrigin[i];
    //    }

    //    StartCoroutine(FadeOut());
    //}

    public void InitFadeOut()
    {
        StopCoroutine(fadeOut);
        fadeOut = FadeOut();

        for (int i = 0; i < myImage.Length; i++)
        {
            myImage[i].color = imageOrigin[i];
        }

        for (int i = 0; i < myText.Length; i++)
        {
            myText[i].color = textOrigin[i];
        }

        StartCoroutine(fadeOut);
    }

    IEnumerator FadeOut()
    {
        Color[] imageColor = new Color[myImage.Length];
        Color[] textColor = new Color[myText.Length];

        for (int i = 0; i < myImage.Length; i++)
        {
            imageColor[i] = myImage[i].color;
        }

        for (int i = 0; i < myText.Length; i++)
        {
            textColor[i] = myText[i].color;
        }

        while (myImage[0].color.a >= 0f)
        {
            for(int i = 0; i < myImage.Length; i++)
            {
                imageColor[i].a -= Time.deltaTime * fadeOutspeed;
                myImage[i].color = imageColor[i];
            }

            for (int i = 0; i < myText.Length; i++)
            {
                textColor[i].a -= Time.deltaTime * fadeOutspeed;
                myText[i].color = textColor[i];
            }

            yield return null;
        }

        //transform.gameObject.SetActive(false);
    }
}
