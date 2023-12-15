using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    #region 다이얼로그 매직넘버 관련 ID
    // ShowDialogue
    // 01 ~ 12 : 튜토리얼 토끼
    // 13 ~ 15 : 양
    // 16 ~ 18 : 꿀벌
    // 19 ~ 21 : 공룡
    // 22 ~ 24 : 은둔형
    // 25 ~ 27 : 말
    // 28 ~ 30 : 뿔고래
    // 31 ~ 33 : 공주
    // 34 ~ 36 : 모험가
    #endregion
    public PlayerInteractNpc testPlayer;

    public GameObject dialogueUI;
    public GameObject questionUI;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    private DialogueTypingEffect myTextEffect;
    public bool isResponse = false;

    [Header("TextSound")]
    //12.15 사운드 작업을 위한 추가
    private AudioSource myAduio = default;
    private AudioClip textSound = default;

    

    void Start()
    {
        myAduio = GetComponent<AudioSource>();
        myAduio.clip = textSound;
        myTextEffect = GetComponent<DialogueTypingEffect>();
        CloseDialogueUI();
        CloseTutoQuestion();
    }
    
    public void ShowTutoDialogue(int keyNum)
    {
        OpenDialogueUI();
        isResponse = true;
        StartCoroutine(StepThroughDialogue(keyNum));
    }

    public void ShowDialogue(int keyNum)
    {
        OpenDialogueUI();
        StartCoroutine(StepThroughDialogue(keyNum));
    }
    private IEnumerator StepThroughDialogue(int keyNum)
    {
        if (keyNum == 0)
        {
            yield break;
        }
        while (true)
        {
            string id = DialogueDBManager.instance.dialogueDic[keyNum].dialogueID;
            string nextId = DialogueDBManager.instance.dialogueDic[keyNum].nextTextNum;
            string[] contexteArray = DialogueDBManager.instance.dialogueDic[keyNum].contextes;

            for (int i = 0; i < contexteArray.Length; i++)
            {
                string dialogue = DialogueDBManager.instance.dialogueDic[keyNum].contextes[i];
                dialogueBox.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
                    DialogueDBManager.instance.dialogueDic[keyNum].speakerName.Trim();
                //yield return myTextEffect.Run(dialogue, dialogueText);
                yield return RunTypingEffect(dialogue);
                
                dialogueText.text = dialogue;

                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            
            if(isResponse)
            {
                //TODO 추가적인 넣고싶은 기능 넣으면 될 듯
                OpenTutoQuestion();
            }

            if (nextId != "")
            {
                keyNum = int.Parse(nextId);
            }
            else if (nextId == "")
            {
                // TODO 대화창 닫고 플레이어 이동제한 해제
                Debug.Log("여기온거면 끝난거임");
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                if (isResponse == false)
                {
                    CloseDialogueUI();
                    //플레이어 움직임 다시 제어 // TODO 캐릭터 움직임 제어
                    //testPlayer.isMove = true;
                }
                break;
            }
            
            Debug.Log("이건 마지막 eKey 위에 디버그");
        }
        yield break;
    }
    private IEnumerator RunTypingEffect(string dialouge)
    {
        myTextEffect.Run(dialouge, dialogueText, myAduio, 1); //매직넘버 톤리스트 결정하는거임
        while (myTextEffect.isTypingRunning)
        {
            yield return null;
            if(Input.GetKeyDown(KeyCode.E))
            {
                myTextEffect.Stop();
            }
        }
    }

    public void OpenDialogueUI()
    {
        dialogueUI.SetActive(true);
    }
    public void CloseDialogueUI() 
    {
        dialogueUI.SetActive(false);
    }

    public void  OpenTutoQuestion()
    {
        questionUI.SetActive(true);
    }
    public void CloseTutoQuestion()
    {
        questionUI.SetActive(false);
    }

    public void ChangeResponeBoolValue(bool value)
    {
        isResponse = value;
    }

    //12.12 추가 
    private IEnumerator StepThroughStateDialogue(int keyNum)
    {
        if (keyNum == 0)
        {
            yield break;
        }
        while (true)
        {
            string id = DialogueDBManager.instance.statusDialogueDic[keyNum].dialogueID;
            string nextId = DialogueDBManager.instance.statusDialogueDic[keyNum].nextTextNum;
            string[] contexteArray = DialogueDBManager.instance.statusDialogueDic[keyNum].contextes;

            for (int i = 0; i < contexteArray.Length; i++)
            {
                string dialogue = DialogueDBManager.instance.statusDialogueDic[keyNum].contextes[i];
                dialogueBox.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
                    DialogueDBManager.instance.statusDialogueDic[keyNum].speakerName.Trim();
                //yield return myTextEffect.Run(dialogue, dialogueText);
                Debug.Log(dialogue);
               
                yield return RunTypingEffect(dialogue);

                dialogueText.text = dialogue;

                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }

            if (isResponse)
            {
                //TODO 추가적인 넣고싶은 기능 넣으면 될 듯
                OpenTutoQuestion();
            }

            if (nextId != "" && nextId != null)
            {
                keyNum = int.Parse(nextId);
            }
            else if (nextId == "" || nextId == null)
            {
                // TODO 대화창 닫고 플레이어 이동제한 해제
                Debug.Log("여기온거면 끝난거임");
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                if (isResponse == false)
                {
                    CloseDialogueUI();
                    //플레이어 움직임 다시 제어
                    //testPlayer.isMove = true;
                }
                break;
            }

            Debug.Log("이건 마지막 eKey 위에 디버그");
        }
        yield break;
    }

    public void ShowStateTutorialDialogue(int keyNum)
    {
        OpenDialogueUI();
        isResponse = true;
        StartCoroutine(StepThroughStateDialogue(keyNum));
    }

    public void ShowStateDialogue(int keyNum)
    {
        OpenDialogueUI();
        StartCoroutine(StepThroughStateDialogue(keyNum));
    }

    public void CheckStateDialogue(int keyNum, npcState _state)
    {
        //status
        // 0 = 기본적인 상태
        // 1 = 접착제 묻은 상태
        // 2 = 물건 붙은 상태

        if (_state == npcState.objectAttached)
        {
            
            ShowStateDialogue(keyNum + 2);
        }
        else if (_state == npcState.glued)
        {
            ShowStateDialogue(keyNum + 1);
        }
        else
        {
            ShowStateDialogue(keyNum);
        }
    }
    public void CheckStateTutorialDialogue(npcState _state)
    {
        //status
        // 0 = 기본적인 상태
        // 1 = 접착제 묻은 상태
        // 2 = 물건 붙은 상태

        if (_state == npcState.objectAttached)
        {
            ShowStateDialogue(12);
        }
        else if (_state == npcState.glued)
        {
            ShowStateDialogue(11);
        }
        else
        {
            ShowStateTutorialDialogue(1); // 토끼 첫번째 대사 
        }
    }

   
}
