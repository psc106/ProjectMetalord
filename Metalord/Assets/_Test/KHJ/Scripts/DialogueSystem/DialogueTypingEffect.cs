using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTypingEffect : MonoBehaviour
{
    private float textSpeed = 10f;

    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(WriteEffect(textToType, textLabel));
    }

    public IEnumerator WriteEffect(string textToType, TMP_Text textLabel)
    {
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
        }
        textLabel.text = textToType;
    }
}
