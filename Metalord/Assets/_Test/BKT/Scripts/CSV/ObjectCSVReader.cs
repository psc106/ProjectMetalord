using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCSVReader : MonoBehaviour
{
    [SerializeField] private GameObject ItemUIObjectPrefab; // 복사하여 생성할 프리펩

    private List<Dictionary<string, object>> objectCSV = new List<Dictionary<string, object>>();

    void Start()
    {
        objectCSV = CSVReader_KT.Read("CSV/ObjectCSV");

        // CSV 정보를 BuffInfo의 리스트에 읽어들입니다.
        for(int i = 0; i < objectCSV.Count; i++)
        {            
            GameObject uiObject = Instantiate(ItemUIObjectPrefab, this.transform); // 내 하위 오브젝트로 생성
            uiObject.name = objectCSV[i]["ObjectID"].ToString();

            // 읽은 정보를 각각의 오브젝트에 넣어줌
            uiObject.GetComponent<ObjectInfo>().objectID = objectCSV[i]["ObjectID"].ToString();
            uiObject.GetComponent<ObjectInfo>().objectPlace = objectCSV[i]["Place"].ToString();
            uiObject.GetComponent<ObjectInfo>().objectName = objectCSV[i]["ObjectName"].ToString();
            uiObject.GetComponent<ObjectInfo>().objectDescription = objectCSV[i]["Description"].ToString();
        }
    }
}
