using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    public TMP_Text pageText;

    public GameObject buttonPrefab;
    public DialogueUI dialogueUI;
    public GameObject questionUI;

    private Button nextPageButton;
    private Button previousPageButton;

    int currentPage = 1, multiple = 0;
    int maxPage;

    //public List<TMP_Text> myQuestion = new List<TMP_Text>();
    private List<GameObject> pageList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //현재 3 과 4는 하이어라키 창에서 questionUI 오브젝트의 자식으로 있는 버튼 오브젝트의 순번이며 일단은 임의로 현재 위치한 자리의 순번을 입력해놓았음
        previousPageButton = questionUI.transform.GetChild(3).gameObject.GetComponent<Button>();
        nextPageButton = questionUI.transform.GetChild(4).gameObject.GetComponent<Button>();

        previousPageButton.onClick.AddListener(() => ClickPreviousPage());
        nextPageButton.onClick.AddListener(()=> ClickNextPage());

        CreateQuestion();
        pageList[0].SetActive(true);
    }
   
    private void CreateQuestion()
    {
        //사실 여기 들어갈 필요는 없지만 페이지 텍스트 설정을 하기 위한 변수 선언
        //Debug.Log(DialogueDBManager.instance.questionDic.Count);
        int buttonCount = DialogueDBManager.instance.questionDic.Count;
        //Debug.Log(buttonCount);
        int coloumCount = 4;

        currentPage = 1;
        multiple = 0;
        
        //Debug.Log(multiple);

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
            //Debug.Log("들어옴?");
            GameObject pageObj = new GameObject($"{i+1}Page");
            pageObj.transform.parent = questionUI.transform;

            //컴포넌트 추가
            pageObj.AddComponent<RectTransform>();
            pageObj.AddComponent<GridLayoutGroup>();
            
            //RectTransform 컴포넌트 변수에 추가 및 초기화
            RectTransform objRect = pageObj.GetComponent<RectTransform>();
            objRect.anchoredPosition3D = Vector3.zero;
            objRect.localRotation = Quaternion.identity;
            objRect.localScale = Vector3.one;

            //RectTransform 수치 변경
            objRect.anchoredPosition3D = new Vector3(530f, 250f, 0f);
            objRect.sizeDelta = new Vector2(600f, 500f);

            //GridLayGroup 컴포넌트 변수에 추가 및 초기화
            GridLayoutGroup objSort = pageObj.GetComponent<GridLayoutGroup>();
            objSort.cellSize = new Vector2(600f, 85.4f);
            objSort.startAxis = GridLayoutGroup.Axis.Vertical;
            //정렬 기준점 설정 및 자식 최대 개수 설정 
            objSort.childAlignment = TextAnchor.UpperCenter;
            objSort.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            objSort.constraintCount = coloumCount;
            //패딩 설정
            objSort.padding.left = 20;
            objSort.padding.right = 20;
            objSort.padding.top = 20;
            objSort.padding.bottom = 20;
            //간격 설정
            objSort.spacing = new Vector2(0f, 35f);

            
            //4개의 질문 오브젝트 생성
            for (int j = multiple; j < buttonCount;)
            {
                //Debug.Log("내부 for문 들어옴?");
                //TODO 여기에 버튼prefab 생성하는 구문 추가해야함
                GameObject btnObj = Instantiate(buttonPrefab, pageObj.transform);
                btnObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = 
                    string.Format(DialogueDBManager.instance.questionDic[++j].
                    questionContextes.Trim());
                //TODO 각각 다이얼로그 대화 연결해주는 함수 연결해줘야함
                Button childButton = btnObj.GetComponent<Button>();
                //Debug.LogFormat("{0}<==이게 j값",j);
                //Debug.LogFormat("{0}<==이게 multiple값", multiple);
                //Debug.Log(buttonCount);
                int dialogueNum = j+1;
                childButton.onClick.AddListener(() => dialogueUI.ShowDialogue(dialogueNum));
                childButton.onClick.AddListener(() => dialogueUI.CloseTutoQuestion());
                childButton.onClick.AddListener(() => dialogueUI.ChangeResponeBoolValue(false));
                //Debug.Log(coloumCount);
                //Debug.LogFormat("{0} 얘가 j 값", j);
                if (j == coloumCount)
                {
                    //Debug.Log("여기 내부 for문 if문 안인데 들어옴?");
                    multiple = j;
                    break;
                }
            }
            pageList.Add(pageObj);
            pageObj.SetActive(false);
        }

    }
    
    public void ClickNextPage()
    {
        //Debug.Log(currentPage);
        //처음에 현재 페이지들 다 꺼주고
        for(int i = 0; i < pageList.Count; i++)
        {
            pageList[i].SetActive(false);
        }
        if(currentPage >= maxPage)
        {
            currentPage = maxPage; 
        }
        else
        {
            currentPage++;
        }
        //여기서 최종적인 현재 페이지의 오브젝트를 켜준다.
        pageList[currentPage-1].SetActive(true);
        //페이지 텍스트 갱신
        pageText.text = string.Format("{0}/{1}", currentPage, maxPage);
        //Debug.Log(currentPage);

    }
    public void ClickPreviousPage() 
    {
        //Debug.Log(currentPage);
        //처음에 현재 페이지들 다 꺼주고
        for (int i = 0; i < pageList.Count; i++)
        {
            pageList[i].SetActive(false);
        }
        if (currentPage <= 1) 
        {
            currentPage = 1;
        }
        else
        {
            currentPage--;
        }
        //여기서 최종적인 현재 페이지의 오브젝트를 켜준다.
        pageList[currentPage-1].SetActive(true);
        //페이지 텍스트 갱신
        pageText.text = string.Format("{0}/{1}", currentPage, maxPage);
        //Debug.Log(currentPage);
    }
}
