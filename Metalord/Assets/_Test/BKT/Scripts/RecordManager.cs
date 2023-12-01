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
    private List<Dictionary<string, object>> objectCSV = new List<Dictionary<string, object>>();

    [SerializeField] private GameObject recordPanel;
    [SerializeField] private GameObject descriptionPanel;


    public GameObject[] records { get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        itemUIObjectPrefab = Resources.Load<GameObject>("Prefabs/Object_forRecord");

        objectCSV = CSVReader_KT.Read("CSV/ObjectCSV"); // CSV 파일 읽어들이기
        records = new GameObject[objectCSV.Count]; // CSV 파일에 입력된 정보의 크기만큼의 배열 생성

        InputCSVFileToInfo();
        ReflectObjectInfo();

    }


    /// <summary>
    /// CSVFile을 읽어서 도감 패널에 반영
    /// 231130_배경택
    /// </summary>
    private void InputCSVFileToInfo()
    {
        // CSV 정보를 BuffInfo의 리스트에 읽어들입니다.
        for (int index = 0; index < objectCSV.Count; index++)
        {
            GameObject uiObject = Instantiate(itemUIObjectPrefab, recordPanel.transform); // 내 하위 오브젝트로 생성

            records[index] = uiObject; // 오브젝트를 배열에 저장

            // 읽은 정보를 각각의 오브젝트에 넣어줌
            uiObject.GetComponent<ObjectInfo>().objectID = objectCSV[index]["ObjectID"].ToString();
            uiObject.GetComponent<ObjectInfo>().objectPlace = objectCSV[index]["Place"].ToString();
            uiObject.GetComponent<ObjectInfo>().objectName = objectCSV[index]["ObjectName"].ToString();
            uiObject.GetComponent<ObjectInfo>().objectDescription = objectCSV[index]["Description"].ToString();
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
            records[index].name = records[index].GetComponent<ObjectInfo>().objectID;
            records[index].GetComponent<Image>().sprite = Resources.Load<Sprite>("Object/" + records[index].GetComponent<ObjectInfo>().objectID);
        }
    }
}
