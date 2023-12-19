using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수집 아이템 스크립트
/// 231219 배경택
/// </summary>
public class RecordItem : MonoBehaviour
{
    [SerializeField] int id = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameEventsManager.instance.recordEvents.GetRecordItem(id);
            Debug.Log("Touch" + id);
            Destroy(this.gameObject);
        }
    }
}
