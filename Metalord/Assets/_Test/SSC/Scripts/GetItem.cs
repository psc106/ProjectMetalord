using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    Dictionary<string, int> itemList;

    private void Awake()
    {
        itemList = new Dictionary<string, int>();

        // TODO : csv파일로 읽어올것

        itemList.Add("1001", 0);
        itemList.Add("1002", 0);
        itemList.Add("1003", 0);
    }

    public void ItemGet(string id)
    {
        itemList[id] += 1;
    }

    public void GIvePassItem()
    {
        // 먹은 아이템 갯수 초기화
        foreach (string test in itemList.Keys)
        {
            if (itemList[test] == 0)
            {
                continue;
            }

            itemList[test] = 0;           
        }

        Debug.Log($"아이템 id : 1001, 초기화 갯수 : {itemList["1001"]}");
        Debug.Log($"아이템 id : 1002, 초기화 갯수 : {itemList["1002"]}");
        Debug.Log($"아이템 id : 1003, 초기화 갯수 : {itemList["1003"]}");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            foreach(KeyValuePair<string, int> test in itemList)
            {
                Debug.Log($"아이템 id : {test.Key}, 획득한 갯수 : {test.Value}");
            }

            Debug.Log($"개별로 찍으면? 1001 : {itemList["1001"]}");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            GIvePassItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Debug.Log(other.gameObject.GetComponent<GetFieldItemBase>().Id);
        }
    }
}
