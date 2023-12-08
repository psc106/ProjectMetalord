using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public GameObject recordUI; // 여기에 UI GameObject를 연결합니다.
    public GameObject settingUI;
    private bool isRecordUIVisible = false; 
    private bool isSettingUIVisible = false;

    private void Start()
    {
        //recordUI.SetActive(false);
    }



    void Update()
    {
        // 수집 도감
        if (Input.GetKeyDown(KeyCode.E))
        {
            // E 키를 누르면 UI를 켜기 or 끄기
            isRecordUIVisible = !isRecordUIVisible;
            recordUI.SetActive(isRecordUIVisible);
        }

        // 환경설정창
        if (Input.GetKeyDown(KeyCode.W))
        {
            // W 키를 누르면 UI를 켜기 or 끄기
            isSettingUIVisible = !isSettingUIVisible;
            settingUI.SetActive(isSettingUIVisible);
        }

        // 환경설정창
        if (Input.GetKeyDown(KeyCode.R))
        {
            //DataManager.instance.LoadData();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {           
            // ESC 키를 누르면 UI 끄기
            if (isRecordUIVisible)
            {
                recordUI.SetActive(false);            
                isRecordUIVisible = false;
            }
            else if(isSettingUIVisible)
            {
                settingUI.SetActive(false);
                isSettingUIVisible = false;
            }
        }

        
    }
}
