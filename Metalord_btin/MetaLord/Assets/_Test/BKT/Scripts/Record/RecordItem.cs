using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수집 아이템 스크립트
/// 231219 배경택
/// </summary>
public class RecordItem : MonoBehaviour
{
    [SerializeField] RecordList recordItem;

    private void OnEnable()
    {
        GameEventsManager.instance.dataEvents.onSaveData += SaveData;
        GameEventsManager.instance.dataEvents.onLoadData += LoadData;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dataEvents.onSaveData -= SaveData;
        GameEventsManager.instance.dataEvents.onLoadData -= LoadData;
    }

    private void SaveData()
    {
        // TODO 수집아이템 활성화 여부 저장
    }

    private void LoadData()
    {
        // TODO 수집아이템 비활성화 여부 저장
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameEventsManager.instance.recordEvents.GetRecordItem((int)recordItem);

            // 사운드 추가
            int id = (int)ItemSoundList.GetRecordItemSound;
            SoundManager.instance.PlaySound(GroupList.Item, id);

            //Debug.Log($"수집품 ID : {(int)recordItem}, 이름 : {recordItem}");
            Destroy(this.gameObject);
        }
    }
}
