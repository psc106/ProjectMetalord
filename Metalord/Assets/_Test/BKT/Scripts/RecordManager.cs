using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject pagePanel;
    [SerializeField] private GameObject recordPanel;
    [SerializeField] private GameObject descriptionPanel;

    private const int PAGE_FULL_ITEMCOUNT = 3;


    public GameObject[] records { get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        itemUIObjectPrefab = Resources.Load<GameObject>("Prefabs/Object_forRecord");
        pagePrefab = Resources.Load<GameObject>("Prefabs/Object_forPage");

        objectCSV = CSVReader_KT.Read("CSV/ObjectCSV"); // CSV 파일 읽어들이기
        records = new GameObject[objectCSV.Count]; // CSV 파일에 입력된 정보의 크기만큼의 배열 생성
        pageList = new List<GameObject>();

        makePage(objectCSV.Count);

        InputCSVFileToInfo();
        ReflectObjectInfo();

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
    /// CSVFile을 읽어서 도감 패널에 반영
    /// 231130_배경택
    /// </summary>
    private void InputCSVFileToInfo()
    {
        int pageIndex = 0;

        // CSV 정보를 BuffInfo의 리스트에 읽어들입니다.
        for (int index = 0; index < objectCSV.Count; index++)
        {
            if (index > PAGE_FULL_ITEMCOUNT) pageIndex++; // 아이템카운트가 한페이지 최고 숫자를 넘기면 다음 페이지에 저장

            GameObject uiObject = Instantiate(itemUIObjectPrefab, pageList[pageIndex].transform); // 내 하위 오브젝트로 생성

            records[index] = uiObject; // 오브젝트를 배열에 저장

            // 읽은 정보를 각각의 오브젝트에 넣어줌
            uiObject.GetComponent<ObjectInfo>().id = int.Parse(objectCSV[index]["ID"].ToString());
            uiObject.GetComponent<ObjectInfo>().id_Description = objectCSV[index]["ID_Description"].ToString();
            uiObject.GetComponent<ObjectInfo>().zone = int.Parse(objectCSV[index]["Zone"].ToString());
            uiObject.GetComponent<ObjectInfo>().item_Name = objectCSV[index]["Item_Name"].ToString();
            uiObject.GetComponent<ObjectInfo>().obtained = bool.Parse(objectCSV[index]["Obtained"].ToString());
            uiObject.GetComponent<ObjectInfo>().description = objectCSV[index]["Description"].ToString();
        }
    }

    /// <summary>
    /// UI 오브젝트에 정보를 반영하는 함수
    /// 231130_배경택
    /// </summary>
    private void ReflectObjectInfo()
    {
        for (int index = 0; index < objectCSV.Count; index++)
        {
            records[index].name = records[index].GetComponent<ObjectInfo>().id_Description;
            records[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("Object/" + records[index].GetComponent<ObjectInfo>().id_Description);
        }
    }
}
