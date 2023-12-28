using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        string chechLineC = "/";
        string[] nanugi = textToType.Split(new char[] { ' ' });
        string sumText = string.Empty;

        for (int i = 0; i < nanugi.Length; i++)
        {
            duration = Time.deltaTime;
            charIndex = 0;
            Debug.LogFormat("{0} <<< 이게 sumText", sumText);
            //Debug.Log(nanugi[i]);
            if (nanugi[i] == chechLineC)
            {
                //Debug.Log("같으면 들어오는건데");
                sumText += "\n";
                continue;
            }
            while (charIndex < nanugi[i].Length)
            {
                duration += Time.deltaTime * textSpeed;
                charIndex = Mathf.FloorToInt(duration);
                charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

                textLabel.text = sumText + nanugi[i].Substring(0, charIndex);
                //Debug.Log(textLabel.text);
                yield return null;
                //Debug.LogFormat("{0} <== This is chaiIndex ", charIndex);
            }
            sumText = string.Empty;
            sumText += textLabel.text + " ";
        }

        //StartCoroutine(TextSoundEffect(textToType,myAduio,textSound));
        isTypingRunning = false;
        Debug.Log("WriteEffect 끝났습니다?");
        fadeImgae.SetActive(true);
        //textLabel.text = sumText;
    }

    public IEnumerator TextSoundEffect(string textToType,AudioSource myAudio, int toneNumber)
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
                //myAudioClip = ChooseRandomSound(toneNumber);
                //myAduio.PlayOneShot(myAudioClip);
                PlayNpcSound(toneNumber);
                yield return new WaitForSeconds(0.085f);
            }
            yield return new WaitForSeconds(0.06f);
        }
        //for (int i = 0;  i < textToType.Length; i++)
        //{
        //    Debug.LogFormat("{0} <== 텍스트 길이", textToType.Length);
        //    myAudioClip = ChooseRandomSound(toneNumber);
        //    myAduio.PlayOneShot(myAudioClip);
        //    yield return new WaitForSeconds(0.085f);
        //}
    }

    private void PlayNpcSound(int toneNumber)
    {
        if(toneNumber == 0)
        {
            randomNumber = UnityEngine.Random.Range(0, boldTone.Length);
            
        }
        else if( toneNumber == 1) 
        {
            randomNumber = 10 + UnityEngine.Random.Range(0, middleTone.Length);
        }
        else
        {
            randomNumber = 20 + UnityEngine.Random.Range(0, highTone.Length);
        }
        SoundManager.instance.PlaySound(GroupList.Npc, randomNumber);
    }

    //TODO 삭제예정
    //private void Test()
    //{
    //    int id = (int)NpcSoundList.lowTone01;
    //    SoundManager.instance.PlaySound(GroupList.Npc, id);
    //}
    //private AudioClip ChooseRandomSound(int toneNumber)
    //{
    //    AudioClip myClip;
    //    if (toneNumber == 0)
    //    {
    //        randomNumber = UnityEngine.Random.Range(0, boldTone.Length);
    //        myClip = boldTone[randomNumber];
    //    }
    //    else if (toneNumber == 1)
    //    {
    //        randomNumber = UnityEngine.Random.Range(0, middleTone.Length);
    //        myClip = middleTone[randomNumber];
    //    }
    //    else
    //    {
    //        randomNumber = UnityEngine.Random.Range(0, highTone.Length);
    //        myClip = highTone[randomNumber];
    //    }

    //    return myClip;
    //}


    //TEST TEXT WRITTER 
    public IEnumerator WriteTest(string textToType, TMP_Text textLabel)
    {
        isTypingRunning = true;
        textLabel.text = string.Empty;

        float duration = Time.deltaTime;
        int charIndex = 0;
        //string boldEnterCheck = "<b>";
        //string boldExitCheck = "</b>";
        //string colorEnterCheck = "<color=#80A0FF>";
        //string colorExitCheck = "</color>";
        string chechLineC = "/";
        string[] nanugi = textToType.Split(new char[] {' '});
        string sumText = string.Empty;

        for (int i = 0; i < nanugi.Length; i++)
        {
            duration = Time.deltaTime;
            charIndex = 0;
            Debug.LogFormat("{0} <<< 이게 sumText", sumText);
            //Debug.Log(nanugi[i]);
            if (nanugi[i] == chechLineC)
            {
                //Debug.Log("같으면 들어오는건데");
                sumText += "\n";
                continue;
            }
            #region 굵기 및 색깔 조건 쓸 경우 주석처리
            //else if (nanugi[i] == boldEnterCheck)
            //{
            //    sumText += boldEnterCheck;
            //    continue;
            //}
            //else if (nanugi[i] == boldExitCheck)
            //{
            //    sumText += boldExitCheck;
            //    continue;
            //}
            //else if(nanugi[i] == colorEnterCheck)
            //{
            //    sumText += colorEnterCheck;
            //    continue;
            //}
            //else if (nanugi[i] == colorExitCheck)
            //{
            //    sumText += colorExitCheck;
            //    continue;
            //}
            #endregion
            while (charIndex < nanugi[i].Length)
            {
                duration += Time.deltaTime * textSpeed;
                charIndex = Mathf.FloorToInt(duration);
                charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);


                //textToType.Substring(0, charIndex);



                textLabel.text = sumText + nanugi[i].Substring(0, charIndex);
                yield return null;
                //Debug.Log(textLabel.text);
                //yield return null;
                //Debug.LogFormat("{0} <== This is chaiIndex ", charIndex);
            }
            sumText = string.Empty;
            sumText += textLabel.text + " ";
        }

        ////StartCoroutine(TextSoundEffect(textToType,myAduio,textSound));


        //isTypingRunning = false;
        ////Debug.Log("지금 writeEffect 언제 되는거지?");
        //fadeImgae.SetActive(true);
        ////textLabel.text = textToType;
    }


}
