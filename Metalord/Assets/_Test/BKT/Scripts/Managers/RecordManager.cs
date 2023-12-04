using System;
using System.Collections;
using System.Collections.Generic;
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

    private const int PAGE_FULL_ITEMCOUNT = 3;


    public GameObject[] records { get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        itemUIObjectPrefab = Resources.Load<GameObject>("Prefabs/Object_ForRecordObject");
        pagePrefab = Resources.Load<GameObject>("Prefabs/Object_ForPage");

        objectCSV = CSVReader_KT.Read("CSV/ObjectCSV"); // CSV 파일 읽어들이기
        recordObjectInfos = new RecordObjectInfo[objectCSV.Count]; // CSV 파일의 크기만큼 배열 크기 생성
        //records = new GameObject[objectCSV.Count]; // CSV 파일에 입력된 정보의 크기만큼의 배열 생성
        pageList = new List<GameObject>();

        InputCSVFileToInfo(); // 초기화 레코드 정보 저장
        makePage(objectCSV.Count); //초기화 페이지 수 계산
        makeRecordObject(recordObjectInfos); //초기화 도감 오브젝트 구성
        //ReflectObjectInfo();
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

            // 읽은 정보를 각각의 오브젝트에 넣어줌
            //uiObject.GetComponent<ObjectInfo>().id = int.Parse(objectCSV[index]["ID"].ToString());
            //uiObject.GetComponent<ObjectInfo>().id_Description = objectCSV[index]["ID_Description"].ToString();
            //uiObject.GetComponent<ObjectInfo>().zone = int.Parse(objectCSV[index]["Zone"].ToString());
            //uiObject.GetComponent<ObjectInfo>().item_Name = objectCSV[index]["Item_Name"].ToString();
            //uiObject.GetComponent<ObjectInfo>().obtained = bool.Parse(objectCSV[index]["Obtained"].ToString());
            //uiObject.GetComponent<ObjectInfo>().description = objectCSV[index]["Description"].ToString();
        }
    }

    /// <summary>
    /// 도감 아이템 개수에 맞춰서 페이지 생성
    /// 231204 배경택
    /// </summary>
    /// <param name="ItemCount"> 도감에 표시될 아이템 개수 </param>
    private void makePage(int ItemCount)
    {
        int temp = ItemCount; //임시 숫자 초기화
        while(temp > 0)
        {
            temp -= PAGE_FULL_ITEMCOUNT; // 한 페이지에 들어가는 총 도감아이템 개수만큼 빼주면서 생성

            GameObject page = Instantiate(pagePrefab, pagePanel.transform); // 페이지아래로 생성
            page.SetActive(false); // 페이지 비활성화
            pageList.Add(page); // 리스트에 페이지 추가
        }
    }

    /// <summary>
    /// 도감 목록 생성
    /// 231204 배경택
    /// </summary>
    /// <param name="_recordObjectInfos"> 레코드오브젝트 정보배열을 페이지에 생성 </param>
    private void makeRecordObject(RecordObjectInfo[] _recordObjectInfos)
    {
        for (int infoIndex = 0; infoIndex < _recordObjectInfos.Length; infoIndex++)
        {
            int pageIndex = infoIndex / PAGE_FULL_ITEMCOUNT; 

            GameObject recordObject = Instantiate(itemUIObjectPrefab, pageList[pageIndex].transform);
            recordObject.GetComponent<RecordObject>().recordInfo = _recordObjectInfos[infoIndex];
        }
        
    }



    

    /// <summary>
    /// UI 오브젝트에 정보를 반영하는 함수
    /// 231130_배경택
    /// </summary>
    //private void ReflectObjectInfo()
    //{
    //    for (int index = 0; index < objectCSV.Count; index++)
    //    {
    //        records[index].name = records[index].GetComponent<ObjectInfo>().id_Description;
    //        records[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("Object/" + records[index].GetComponent<ObjectInfo>().id_Description);
    //    }
    //}
}
