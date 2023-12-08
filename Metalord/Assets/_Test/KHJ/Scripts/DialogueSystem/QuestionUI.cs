using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class QuestionUI : MonoBehaviour
{
    public TMP_Text pageText;

    public GameObject buttonPrefab;
    public GameObject questionUI;

    public List<TMP_Text> myQuestion = new List<TMP_Text>();
    

    // Start is called before the first frame update
    void Start()
    {
        //pageText = GetComponent<TMP_Text>();
        //CreateQuestion();
        for(int i = 0; i < myQuestion.Count; i++)//DialogueDBManager.instance.dialogueQuestions.Count; i++)
        {
            myQuestion[i].text = DialogueDBManager.instance.dialogueQuestions[i];
        }

    }
    //TODO 마지막에는 이걸로 바꿀 예정 
    private void CreateQuestion()
    {
        Debug.Log(DialogueDBManager.instance.questionDic.Count);
        int buttonCount = DialogueDBManager.instance.questionDic.Count;
        Debug.Log(buttonCount);
        int coloumCount = 4;

        int currentPage = 1,  multiple = 0;
        int maxPage;
        Debug.Log(multiple);

        if(buttonCount % coloumCount == 0)
        {
            maxPage = buttonCount / coloumCount;
        }
        else
        {
            maxPage = (buttonCount / coloumCount) + 1;
        }

        //페이지 텍스트 설정
        pageText.text = string.Format("{0}/{1}",currentPage, maxPage);

        //하나의 부모 오브젝트와 4개의 질문 버튼 오브젝트를 들고 있는 반복문
        for (int i = 0; i < maxPage; i++)
        {
            Debug.Log("들어옴?");
            GameObject pageObj = new GameObject($"{i+1}Page");
            pageObj.transform.parent = questionUI.transform;
            
            //grid 들어가야함 
            for (int j = multiple; j < buttonCount;)
            {
                Debug.Log("내부 for문 들어옴?");
                //TODO 여기에 버튼prefab 생성하는 구문 추가해야함
                GameObject btnObj = Instantiate(buttonPrefab, pageObj.transform);
                btnObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = 
                    string.Format(DialogueDBManager.instance.questionDic[++j].
                    questionContextes.Trim());
                Debug.Log(coloumCount);
                Debug.LogFormat("{0} 얘가 j 값", j);
                if (j == coloumCount)
                {
                    Debug.Log("여기 내부 for문 if문 안인데 들어옴?");
                    multiple = j;
                    break;
                }
            }
        }

    }

}
