using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTypingEffect : MonoBehaviour
{
    //AudioSource myAduio = default;
    
    private float textSpeed = 10f;
    public GameObject fadeImgae = default;
    public bool isTypingRunning {  get; private set; }

    private Coroutine typingCoroutine;
    private Coroutine textSoundCoroutine;

    [Header("RandomSound")]
    public int randomNumber = 0;
    public AudioClip[] boldTone = default;
    public AudioClip[] middleTone = default;
    public AudioClip[] highTone = default;

    private AudioClip myAudioClip = default;

    public void Run(string textToType, TMP_Text textLabel, AudioSource myAduio, int toneNumber)
    {
        typingCoroutine = StartCoroutine(WriteEffect(textToType, textLabel));
        textSoundCoroutine = StartCoroutine(TextSoundEffect(textToType, myAduio, toneNumber));
    }
    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        StopCoroutine(textSoundCoroutine);
        isTypingRunning = false;
    }

    //public Coroutine Run(string textToType, TMP_Text textLabel)
    //{
    //    return StartCoroutine(WriteEffect(textToType, textLabel));
    //}
    public IEnumerator WriteEffect(string textToType, TMP_Text textLabel)
    {
        isTypingRunning = true;
        textLabel.text = string.Empty;
        
        float duration = Time.deltaTime;
        int charIndex = 0;
        //StartCoroutine(TextSoundEffect(textToType,myAduio,textSound));
        while (charIndex < textToType.Length) 
        {
            duration += Time.deltaTime * textSpeed;
            charIndex = Mathf.FloorToInt(duration);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textLabel.text = textToType.Substring(0, charIndex);
            //Debug.Log(textLabel.text);
            yield return null;
            //Debug.LogFormat("{0} <== This is chaiIndex ", charIndex);
        }
        isTypingRunning = false;
        Debug.Log("지금 writeEffect 언제 되는거지?");
        fadeImgae.SetActive(true);
        //textLabel.text = textToType;
    }

    public IEnumerator TextSoundEffect(string textToType, AudioSource myAduio, int toneNumber)
    {
        string[] temporaryText = textToType.Split(new char[] { ' ' });
        //Debug.Log(temporaryText.Length);
        for(int i = 0; i < temporaryText.Length; i++)
        {
            //string[] row = testText[i].Split(new char[] { ' ' });
            //Debug.Log(temporaryText[i].Length);
            //Debug.LogFormat("{0} <=== 텍스트", temporaryText[i]);
            for (int j = 0; j < temporaryText[i].Length; j++)
            {
                myAudioClip = ChooseRandomSound(toneNumber);
                myAduio.PlayOneShot(myAudioClip);
                yield return new WaitForSeconds(0.085f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        //for (int i = 0;  i < textToType.Length; i++)
        //{
        //    Debug.LogFormat("{0} <== 텍스트 길이", textToType.Length);
        //    myAudioClip = ChooseRandomSound(toneNumber);
        //    myAduio.PlayOneShot(myAudioClip);
        //    yield return new WaitForSeconds(0.085f);
        //}
    }


    private AudioClip ChooseRandomSound(int toneNumber)
    {
        AudioClip myClip;
        if (toneNumber == 0)
        {
            randomNumber = UnityEngine.Random.Range(0, boldTone.Length);
            myClip = boldTone[randomNumber];
        }
        else if (toneNumber == 1)
        {
            randomNumber = UnityEngine.Random.Range(0, middleTone.Length);
            myClip = middleTone[randomNumber];
        }
        else
        {
            randomNumber = UnityEngine.Random.Range(0, highTone.Length);
            myClip = highTone[randomNumber];
        }

        return myClip;
    }
}
