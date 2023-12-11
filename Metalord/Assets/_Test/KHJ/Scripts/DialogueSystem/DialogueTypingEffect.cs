using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTypingEffect : MonoBehaviour
{
    private float textSpeed = 10f;

    public bool isTypingRunning {  get; private set; }

    private Coroutine typingCoroutine;
    public void Run(string textToType, TMP_Text textLabel)
    {
        typingCoroutine = StartCoroutine(WriteEffect(textToType, textLabel));
    }
    public void Stop()
    {
        StopCoroutine(typingCoroutine);
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
        while (charIndex < textToType.Length) 
        {
            duration += Time.deltaTime * textSpeed;
            charIndex = Mathf.FloorToInt(duration);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textLabel.text = textToType.Substring(0, charIndex);
            yield return null;
            //Debug.LogFormat("{0} <== This is chaiIndex ", charIndex);
        }

        isTypingRunning = false;
        //textLabel.text = textToType;
    }
}
