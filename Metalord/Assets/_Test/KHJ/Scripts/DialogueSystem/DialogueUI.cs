using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    DialogueTypingEffect myTextEffect;

    private Dialogue myDialogue;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GetDialogue(int keyNum)
    {
        if(keyNum == 0) {return;}
        string id = DialogueDBManager.instance.dialogueDic[keyNum].dialogueID;
        string nextId = DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum;
        
        if(DialogueDBManager.instance.dialogueDic[keyNum].contextes.Length <= 1)
        {

        }
        
    }

    private IEnumerator StepThroughDialogue(int keyNum)
    {
        if (keyNum == 0) 
        {
            yield break;
        }
        string id = DialogueDBManager.instance.dialogueDic[keyNum].dialogueID;
        string nextId = DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum;

        for(int i  = 0; i < DialogueDBManager.instance.dialogueDic[keyNum].contextes.Length )
        {
            //yield return 
        }

    }


}
