using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyExplainCanvas : MonoBehaviour
{
    private bool isConfirm = false;

    private void Awake()
    {
        GameEventsManager.instance.dataEvents.onSaveData += SaveObject;
        GameEventsManager.instance.dataEvents.onLoadData += LoadObject;
    }

    private void Start()
    {
        if (isConfirm)
        {
            gameObject.SetActive(false); // 이미 확인되었다면 비활성화
            Controller_Physics.SwitchCameraLock(false);
        }
        else Controller_Physics.SwitchCameraLock(true); // 확인되지 않았다면 플레이어 움직임, 카메라 제한
    }

    private void OnDisable()
    {
        isConfirm = true;
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.dataEvents.onSaveData -= SaveObject;
        GameEventsManager.instance.dataEvents.onLoadData -= LoadObject;        
    }

    private void SaveObject()
    {
        DataManager.instance.savedGamePlayData.ui_keyExplain = isConfirm;

    }

    private void LoadObject()
    {
        isConfirm = DataManager.instance.savedGamePlayData.ui_keyExplain;
    }
}
