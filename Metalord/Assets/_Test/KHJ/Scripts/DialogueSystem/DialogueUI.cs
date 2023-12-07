using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    private DialogueTypingEffect myTextEffect;

    private Dialogue myDialogue;

    void Start()
    {
        myTextEffect = GetComponent<DialogueTypingEffect>();
        //myTextEffect.Run("TEst TEST TEST TEST STESTSETS", dialogueText);
        StartCoroutine(StepThroughDialogue(4));
    }

    void Update()
    {
        
    }

    private IEnumerator StepThroughDialogue(int keyNum)
    {
        if (keyNum == 0) 
        {
            yield break;
        }
        //TODO 변수에 DialogueDBManager.instance.dialogueDic[keyNum] 담아서 코드 보기 편하게 해야함

        //do
        while (true) 
        {
            string id = DialogueDBManager.instance.dialogueDic[keyNum].dialogueID;
            string nextId = DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum;

            for (int i = 0; i < DialogueDBManager.instance.dialogueDic[keyNum].contextes.Length; i++)
            {
                string dialogue = DialogueDBManager.instance.dialogueDic[keyNum].contextes[i];
                yield return myTextEffect.Run(dialogue, dialogueText);

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }

            if (DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum.ToString() != "")
            {
                //Debug.LogFormat("{0}<<< 다음 번호로 넘어가기 위해서 그 값 확인", DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum);
                ////TODO 
                //if (DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum.ToString() == "")
                //{
                //    Debug.Log("부숴버리기");
                //    break;
                //}
                Debug.LogFormat("{0}<<< 다음 번호로 넘어가기 위해서 그 값 파서 전 확인", DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum);

                Debug.LogFormat("{0} === 이게 파서 전 keynum", keyNum);
                keyNum = int.Parse(DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum);
                Debug.LogFormat("{0}<<< 다음 번호로 넘어가기 위해서 그 값 파서 후 확인", DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum);

                Debug.LogFormat("{0} === 이게 파서 후 keynum", keyNum);

                Debug.Log("여기 들어옴?");
            }
            else if(DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum.ToString() == "")
            {
                // TODO 대화창 닫고 플레이어 이동제한 해제
                Debug.Log("여기온거면 끝난거임");
                break;  //굳이 해줘야하나 ?
            }
        }
        yield break;
        //while (DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum == "");
        //Debug.Log("여기온거면 끝난거임real");
    }

}
