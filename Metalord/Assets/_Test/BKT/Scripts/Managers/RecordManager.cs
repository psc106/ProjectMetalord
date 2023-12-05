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
    private RecordObjectInfo[] recordObjectInfos;

    [SerializeField] private GameObject pagePanel;
    [SerializeField] private GameObject recordPanel;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;

    private GameObject name;
    private GameObject zone;
    private GameObject description;
    private string s_zone;

    private const int PAGE_FULL_ITEMCOUNT = 3;

    private const string RESET_NAME = "Not Select";
    private const string RESET_Zone = "Not Select";
    private const string RESET_Description = "Not Select";

    private int checkRecordInfoId;
    private const int ID_RESET = -1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        // 사전 우측 정보창 게임오브젝트 가져오기
        name = GameObject.Find("Text(Info_Name)");
        zone = GameObject.Find("Text(Info_Zone)");
        description = GameObject.Find("Text(Info_Description)");
        checkRecordInfoId = ID_RESET;

        itemUIObjectPrefab = Resources.Load<GameObject>("Prefabs/Object_ForRecordObject");
        pagePrefab = Resources.Load<GameObject>("Prefabs/Object_ForPage");

        objectCSV = CSVReader_KT.Read("CSV/ObjectCSV"); // CSV 파일 읽어들이기
        recordObjectInfos = new RecordObjectInfo[objectCSV.Count]; // CSV 파일의 크기만큼 배열 크기 생성
        //records = new GameObject[objectCSV.Count]; // CSV 파일에 입력된 정보의 크기만큼의 배열 생성
        pageList = new List<GameObject>();

        InputCSVFileToInfo(); // 초기화 레코드 정보 저장
        MakePage(objectCSV.Count); //초기화 페이지 수 계산
        MakeRecordObject(recordObjectInfos); //초기화 도감 오브젝트 구성
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

            recordObjectInfos[index] = recordInfo; // 도감 정보를 저장해서 배열로 관리
        }
    }

    /// <summary>
    /// 도감 아이템 개수에 맞춰서 페이지 생성
    /// 231204 배경택
    /// </summary>
    /// <param name="ItemCount"> 도감에 표시될 아이템 개수 </param>
    private void MakePage(int ItemCount)
    {
        while(ItemCount > 0)
        {
            ItemCount -= PAGE_FULL_ITEMCOUNT; // 한 페이지에 들어가는 총 도감아이템 개수만큼 빼주면서 생성

            GameObject page = Instantiate(pagePrefab, pagePanel.transform); // 페이지패널 아래로 생성
            page.SetActive(false); // 페이지 비활성화
            pageList.Add(page); // 리스트에 페이지 추가
        }
    }

    /// <summary>
    /// 도감 목록 생성
    /// 231204 배경택
    /// </summary>
    /// <param name="_recordObjectInfos"> 레코드오브젝트 정보배열을 페이지에 생성 </param>
    private void MakeRecordObject(RecordObjectInfo[] _recordObjectInfos)
    {
        for (int infoIndex = 0; infoIndex < _recordObjectInfos.Length; infoIndex++)
        {
            int pageIndex = infoIndex / PAGE_FULL_ITEMCOUNT; 

            GameObject recordObject = Instantiate(itemUIObjectPrefab, pageList[pageIndex].transform);
            recordObject.GetComponent<RecordObject>().recordInfo = _recordObjectInfos[infoIndex];
        }
        
    }


    /// <summary>
    /// 도감정보에 선택된 도감아이템 정보 입력
    /// 231205 배경택
    /// </summary>
    public void InputRecordInfo(RecordObjectInfo recordInfo)
    {
        if (checkRecordInfoId == recordInfo.id) // 중복으로 같은 아이디값이 들어올 경우
        {
            DeleteRecordInfo(); return; // 도감 우측 텍스트정보 지우기
        }
        checkRecordInfoId = recordInfo.id; // 도감 ID값 저장

        name.GetComponent<TMP_Text>().text = recordInfo.item_Name;
        description.GetComponent<TMP_Text>().text = recordInfo.description;

        switch (recordInfo.zone)
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
    }

    /// <summary>
    /// 도감정보 내용 지우기
    /// 231205 배경택
    /// </summary>
    public void DeleteRecordInfo()
    {
        checkRecordInfoId = ID_RESET;
        name.GetComponent<TMP_Text>().text = RESET_NAME;
        description.GetComponent<TMP_Text>().text = RESET_Description;
        zone.GetComponent<TMP_Text>().text = RESET_Zone;
    }
}
