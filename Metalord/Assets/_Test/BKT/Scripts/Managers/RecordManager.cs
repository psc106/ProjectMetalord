using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 수집,도감 매니저
/// 231130_배경택
/// </summary>
public class RecordManager : MonoBehaviour
{
    static public RecordManager instance;

    private GameObject itemUIObjectPrefab; // 복사하여 생성할 프리펩
    private GameObject pagePrefab; // 복사하여 생성할 프리펩
    private List<Dictionary<string, object>> objectCSV = new List<Dictionary<string, object>>();
    private List<GameObject> pageList;

    private Dictionary<int,RecordObjectInfo> recordObjectInfos; // 도감 정보 전체가 저장된 원본

    private List<RecordObjectInfo> tempRecordObjectInfos; // 정렬에 맞춰 변경될 도감 정보 배열

    private int zoneSortIndex;
    private int gotSortIndex;

    [SerializeField] private GameObject pagePanel; //페이지 좌측 페이지가 생성될 기준 패널
    [SerializeField] private GameObject itemName; //페이지 우측 이름 text
    [SerializeField] private GameObject zone; //페이지 우측 지역 text
    [SerializeField] private GameObject description; //페이지 우측 설명 text

    private string s_zone; // 지역저장을 위한 임시변수

    private const int PAGE_FULL_ITEMCOUNT = 3; // 한페이지에 몇개의 아이템을 표시할 것인지

    // 우측 도감 설명 초기화 텍스트
    private const string RESET_NAME = "Not Select";
    private const string RESET_Zone = "Not Select";
    private const string RESET_Description = "Not Select";

    private int checkRecordInfoId;
    private const int ID_RESET = -1;

    // 페이지 번호 및 활성화 비활성화
    private int _pageIndex;
    private int pageIndex
    {
        get
        {
            return _pageIndex;
        }
        set
        {
            // 페이지 인덱스번호에 맞춰서 페이지전환
            pageList[_pageIndex].SetActive(false);
            _pageIndex = value;
            pageList[_pageIndex].SetActive(true);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        // 도감 우측 정보창 게임오브젝트 가져오기
        //itemName = GameObject.Find("Text(Info_Name)");
        //zone = GameObject.Find("Text(Info_Zone)");
        //description = GameObject.Find("Text(Info_Description)");
        //pagePanel = GameObject.Find("RecordPagePanel");

        itemUIObjectPrefab = Resources.Load<GameObject>("Prefabs/Object_ForRecordObject");
        pagePrefab = Resources.Load<GameObject>("Prefabs/Object_ForPage");

        checkRecordInfoId = ID_RESET;

        objectCSV = CSVReader_KT.Read("CSV/ObjectCSV"); // CSV 파일 읽어들이기 TODO 데이터베이스
        recordObjectInfos = new Dictionary<int, RecordObjectInfo>();
        tempRecordObjectInfos = new List<RecordObjectInfo>();
        pageList = new List<GameObject>();

        // 정렬 인덱스 초기화
        gotSortIndex = 0;
        zoneSortIndex = 0;

        InputCSVFileToInfo(); // 초기화 레코드 정보 저장
        MakePage(objectCSV.Count); //초기화 페이지 수 계산
        MakeRecordObject(recordObjectInfos); //초기화 도감 오브젝트 구성
    }

    private void OnEnable()
    {
        GameEventsManager.instance.recordEvents.onGetRecordItem += GetItem;
        GameEventsManager.instance.recordEvents.onSelectRecord += InputRecordInfo;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.recordEvents.onGetRecordItem -= GetItem;
        GameEventsManager.instance.recordEvents.onSelectRecord -= InputRecordInfo;

    }

    /// <summary>
    /// 레코드 아이템을 먹었을때 호출되는 함수
    /// 231206 배경택
    /// </summary>
    /// <param name="_id"></param>
    private void GetItem(int _id)
    {

        if (recordObjectInfos[_id].id == _id)
        {
            recordObjectInfos[_id].obtained = true;
            GameEventsManager.instance.recordEvents.ReflectRecord(); // 정보 반영 이벤트 발생
        }
        
    }

    private void Start()
    {
        GameEventsManager.instance.recordEvents.ReflectRecord();
    }

    /// <summary>
    /// CSVFile을 읽은 뒤 레코드 오브젝트 정보에 반영
    /// 231130_배경택
    /// </summary>
    private void InputCSVFileToInfo()
    {
        // CSV 정보를 BuffInfo의 리스트에 읽어들입니다.
        for (int index = 0; index < objectCSV.Count; index++)
        {
            RecordObjectInfo recordInfo = new RecordObjectInfo();

            recordInfo.id = int.Parse(objectCSV[index]["ID"].ToString());
            recordInfo.id_Description = objectCSV[index]["ID_Description"].ToString();
            recordInfo.zone = int.Parse(objectCSV[index]["Zone"].ToString());
            recordInfo.item_Name = objectCSV[index]["Item_Name"].ToString();
            recordInfo.obtained = bool.Parse(objectCSV[index]["Obtained"].ToString());
            recordInfo.description = objectCSV[index]["Description"].ToString();
            recordInfo.isSelected = false;

            recordObjectInfos[recordInfo.id] = recordInfo; // 도감 정보를 저장해서 Dictionary로 관리
        }
    }

    /// <summary>
    /// 도감 아이템 개수에 맞춰서 페이지 생성
    /// 231204 배경택
    /// </summary>
    /// <param name="ItemCount"> 도감에 표시될 아이템 개수 </param>
    private void MakePage(int ItemCount)
    {
        if(pageList.Count > 0)
        {
            for(int i = 0; i < pageList.Count; i++)
            {
                Destroy(pageList[i]);
            }
            pageList.Clear();
        }

        if(ItemCount == 0)// 만약 아이템 수량이 없다면
        {
            GameObject page = Instantiate(pagePrefab, pagePanel.transform);
            pageList.Add(page); // 리스트에 페이지 추가
            return;
        }

        while(ItemCount > 0)
        {
            ItemCount -= PAGE_FULL_ITEMCOUNT; // 한 페이지에 들어가는 총 도감아이템 개수만큼 빼주면서 생성

            GameObject page = Instantiate(pagePrefab, pagePanel.transform); // 페이지패널 아래로 생성
            page.SetActive(false); // 페이지 비활성화
            pageList.Add(page); // 리스트에 페이지 추가
        }

        pageList[0].SetActive(true); // 첫 페이지 보이도록 실행
    }

    /// <summary>
    /// 도감 목록 생성
    /// 231204 배경택
    /// </summary>
    /// <param name="_recordObjectInfos"> 수집 아이템 정보 Dictionary </param>
    private void MakeRecordObject(Dictionary<int,RecordObjectInfo> _recordObjectInfos)
    {
        for (int infoIndex = 0; infoIndex < _recordObjectInfos.Count; infoIndex++)
        {
            int tempIndex = infoIndex / PAGE_FULL_ITEMCOUNT; 

            GameObject recordObject = Instantiate(itemUIObjectPrefab, pageList[tempIndex].transform);
            recordObject.GetComponent<RecordObject>().recordInfo = _recordObjectInfos[infoIndex+1];
        }
    }

    /// <summary>
    /// 도감 목록 생성
    /// 231204 배경택
    /// </summary>
    /// <param name="_recordObjectInfos"> 수집 아이템 정보 List </param>
    private void MakeRecordObject(List<RecordObjectInfo> _recordObjectInfos)
    {
        for (int infoIndex = 0; infoIndex < _recordObjectInfos.Count; infoIndex++)
        {
            int tempIndex = infoIndex / PAGE_FULL_ITEMCOUNT;

            GameObject recordObject = Instantiate(itemUIObjectPrefab, pageList[tempIndex].transform);
            recordObject.GetComponent<RecordObject>().recordInfo = _recordObjectInfos[infoIndex];
        }
    }


    /// <summary>
    /// 도감정보에 선택된 도감아이템 정보 입력
    /// 231205 배경택
    /// </summary>
    public void InputRecordInfo(int selectedId)
    {
        for(int i = 1; i < recordObjectInfos.Count; i++)
        {
            if(selectedId == i) continue;
            recordObjectInfos[i].isSelected = false;
        }

        if (checkRecordInfoId == recordObjectInfos[selectedId].id) // 중복으로 같은 아이디값이 들어올 경우
        {
            DeleteRecordInfo(); // 도감 우측 텍스트정보 지우기
            return;
        }
        checkRecordInfoId = recordObjectInfos[selectedId].id; // 도감 ID값 저장

        itemName.GetComponent<TMP_Text>().text = recordObjectInfos[selectedId].item_Name;

        switch (recordObjectInfos[selectedId].zone)
        {
            case 1:
                s_zone = "Kitchen";
                break;
            case 2:
                s_zone = "Living Room";
                break;
            case 3:
                s_zone = "Baby Room";
                break;
        }
        zone.GetComponent<TMP_Text>().text = s_zone;

        // 얻은 도감아이템일 경우에만 설명이 출력됨
        if (recordObjectInfos[selectedId].obtained) description.GetComponent<TMP_Text>().text = recordObjectInfos[selectedId].description;
        else description.GetComponent<TMP_Text>().text = "???";
    }


    /// <summary>
    /// 도감정보 내용 지우기
    /// 231205 배경택
    /// </summary>
    private void DeleteRecordInfo()
    {
        checkRecordInfoId = ID_RESET;
        itemName.GetComponent<TMP_Text>().text = RESET_NAME;
        description.GetComponent<TMP_Text>().text = RESET_Description;
        zone.GetComponent<TMP_Text>().text = RESET_Zone;
    }

    /// <summary>
    /// 우측 버튼 누르면 pageIndex가 증가하며 페이지 변경
    /// 231205 배경택
    /// </summary>
    public  void PushRightButton()
    {
        if (pageIndex < pageList.Count-1)
        {
            pageIndex++;
        }
    }

    /// <summary>
    /// 좌측 버튼 누르면 pageIndex가 감소하며 페이지 변경
    /// 231205 배경택
    /// </summary>
    public void PushLeftButton()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
        }
    }

    /// <summary>
    /// 획득기준으로 정렬하는 함수
    /// 231205 배경택
    /// </summary>
    /// <param name="optionIndex">정렬목록 옵션 인덱스(0: 전체, 1: 획득, 2: 미획득)</param>
    public void SortGot(int optionIndex) 
    {
        gotSortIndex = optionIndex; //정렬 인덱스 저장

        SortTotal();
    }

    /// <summary>
    /// 지역기준으로 정렬하는 함수
    /// 231205 배경택
    /// </summary>
    /// <param name="optionIndex">정렬목록 옵션 인덱스(0: 전체, 1: 주방, 2: 거실, 3: 아기방) </param>
    public void SortZone(int optionIndex)
    {
        zoneSortIndex = optionIndex; // 정렬 인덱스 저장
        SortTotal();
    }

    /// <summary>
    /// 전체 정렬 함수
    /// 231206 배경택
    /// </summary>
    private void SortTotal()
    {
        tempRecordObjectInfos.Clear();

        bool check = false; // 비교를 위한 임시 변수

        if (gotSortIndex == 1) check = true; // 획득된 상태
        else if (gotSortIndex == 2) check = false; //획득되지 않은 상태

        for (int i = 1; i < recordObjectInfos.Count+1; i++) // Dictionary ID값이 1부터 시작하기때문에 i값이 1~Count+1로 설정
        {
            if (recordObjectInfos[i].obtained == check && recordObjectInfos[i].zone == zoneSortIndex)
            {
                tempRecordObjectInfos.Add(recordObjectInfos[i]);
            }
            else if(recordObjectInfos[i].obtained == check && zoneSortIndex == 0)
            {
                tempRecordObjectInfos.Add(recordObjectInfos[i]);
            }
            else if(gotSortIndex == 0 && recordObjectInfos[i].zone == zoneSortIndex)
            {
                tempRecordObjectInfos.Add(recordObjectInfos[i]);
            }
            else if(gotSortIndex == 0 && zoneSortIndex == 0)
            {
                tempRecordObjectInfos.Add(recordObjectInfos[i]);
            }
        }

        // 임시 리스트를 매개변수로 페이지와 오브젝트 생성
        MakePage(tempRecordObjectInfos.Count);
        MakeRecordObject(tempRecordObjectInfos);
    }
}
