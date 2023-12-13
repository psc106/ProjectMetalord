using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestionListTest : MonoBehaviour
{
    private DialogueUI dialogueUI;

    private List<GameObject> tempResponseButtons = new List<GameObject>();

    public string[] questionArray = default;
    

    public Button[] content = default;

    int currentPage = 1, maxPage = 0, multiple = 0;

    public Button previousButton = default;
    public Button nextButton = default;
    int nextBtnNum = -1, prevBtnNum = -2;
    
    void Start()
    {
        //dialogueUI = GetComponent<DialogueUI>();
        //testData.QuestionListParse();
        
    }

    void Update()
    {
        
    }

    //public void ShowResponses(Respoonse[] )

    public void ClickBtn(int num)
    {
        if(num == prevBtnNum)
        {

        }
        else if(num == nextBtnNum)
        {

        }
        else
        {

        }
    }

    public void UpdateQuestionList()
    {
        int questionCount = DialogueDBManager.instance.questionDic.Count;
        questionArray = new string[questionCount];
        for (int i = 0; i < questionCount; i++)
        {
            //string context = DialogueDBManager.instance.questionDic[i];
            //TODO 텍스트 바꿔주고 이벤트 연결해주면 됨  딱 2개임 
            

        }

        //최대페이지
        if (questionArray.Length % content.Length == 0)
        {
            maxPage = questionArray.Length / content.Length;
        }
        else
        {
            maxPage = (questionArray.Length / content.Length) + 1;
        }

        //이전 버튼활성화 관련
        if(currentPage <= 1 )
        {
            previousButton.interactable = false;
        }
        else
        {
            previousButton.interactable = true;
        }
        
        //다음 버튼활성화 관련
        if (currentPage >= maxPage)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable= true;
        }

        multiple = (currentPage - 1) * content.Length;
        for(int i = 0; i < content.Length; i++)
        {
            //질문 내용 버튼 활성화 관련
            if(multiple + 1 < questionArray.Length )
            {
                content[i].interactable = true;
                //content[i].transform.GetChild(0).GetComponent<TMP_Text>().text =  "Question string" 
            }
            else
            {
                content[i].interactable = false;
                content[i].transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            }

            

        }
        //multiple = (currentPage - 1) * CellBtn.Length;
        //for (int i = 0; i < CellBtn.Length; i++)
        //{
        //    CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
        //    CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
        //    CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        //}
    }
   
}
