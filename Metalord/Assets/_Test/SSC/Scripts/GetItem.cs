using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    Dictionary<string, int> itemList;
    Rigidbody rb;

    Vector3 speed;
    private void Awake()
    {
         rb = GetComponent<Rigidbody>();
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

    public void CleaningTrain()
    {
        // 먹은 아이템 갯수 초기화
        foreach (var test in itemList.ToList())
        {
            if (itemList[test.Key] == 0)
            {
                continue;
            }

            itemList[test.Key] = 0;           
        }

        Debug.Log($"아이템 id : 1001, 초기화 갯수 : {itemList["1001"]}");
        Debug.Log($"아이템 id : 1002, 초기화 갯수 : {itemList["1002"]}");
        Debug.Log($"아이템 id : 1003, 초기화 갯수 : {itemList["1003"]}");
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(x,0,z) * 5f;

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach(KeyValuePair<string, int> test in itemList)
            {
                Debug.Log($"아이템 id : {test.Key}, 획득한 갯수 : {test.Value}");
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            GetFieldItemBase obj = other.gameObject.GetComponent<GetFieldItemBase>();
            obj.GetItem();
            Debug.Log($"현재 접촉한 아이템 아이디 : {obj.Id}, 아이템 갯수 : {itemList[obj.Id]}");
        }

        if(other.gameObject.name == "Pulley")
        {
            CleaningTrain();
        }
    }
}
