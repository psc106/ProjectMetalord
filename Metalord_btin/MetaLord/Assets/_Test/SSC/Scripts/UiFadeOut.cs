using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiFadeOut : MonoBehaviour
{
    float fadeOutspeed = 1f;

    //TextMeshProUGUI[] myText;
    Image myImage;

    Color imageOrigin;
    //Color[] textOrigin;


    Coroutine fadeOut;
    //IEnumerator fadeOut;

    private void Awake()
    {
        //myText = GetComponentsInChildren<TextMeshProUGUI>();
        myImage = GetComponent<Image>();

        imageOrigin = myImage.color;
        //textOrigin = new Color[myText.Length];

        //for (int i = 0; i < myImage.Length; i++)
        //{
        //    imageOrigin[i] = myImage[i].color;
        //}

        //for (int i = 0; i < myText.Length; i++)
        //{
        //    textOrigin[i] = myText[i].color;
        //}
    }

    private void Start()
    {
        Color tempColor = myImage.color;
        tempColor.a = 0f;
        myImage.color = tempColor;
        //fadeOut = FadeOut();
        //InitFadeOut();
    }

    public void InitFadeOut()
    {
        if(fadeOut != null)
        {            
            return;
        }

        myImage.color = imageOrigin;
        fadeOut = StartCoroutine(FadeOut());
        //StopCoroutine(fadeOut);
        //fadeOut = FadeOut();

        //for (int i = 0; i < myImage.Length; i++)
        //{
        //    myImage[i].color = imageOrigin[i];
        //}

        //for (int i = 0; i < myText.Length; i++)
        //{
        //    myText[i].color = textOrigin[i];
        //}

        //StartCoroutine(fadeOut);
    }

    IEnumerator FadeOut()
    {
        //Color[] imageColor = new Color[myImage.Length];
        //Color[] textColor = new Color[myText.Length];
        Color imageColor = myImage.color;

        //for (int i = 0; i < myImage.Length; i++)
        //{
        //    imageColor[i] = myImage[i].color;
        //}

        //for (int i = 0; i < myText.Length; i++)
        //{
        //    textColor[i] = myText[i].color;
        //}

        while (myImage.color.a >= 0f)
        {
            
            imageColor.a -= Time.deltaTime * fadeOutspeed;            
            myImage.color = imageColor;

            //for (int i = 0; i < myText.Length; i++)
            //{
            //    textColor[i].a -= Time.deltaTime * fadeOutspeed;
            //    myText[i].color = textColor[i];
            //}

            yield return null;
        }

        fadeOut = null;
        //transform.gameObject.SetActive(false);
    }
}
