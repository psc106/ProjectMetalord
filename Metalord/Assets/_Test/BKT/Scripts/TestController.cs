using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public GameObject recordUI; // 여기에 UI GameObject를 연결합니다.
    private bool isUIVisible = false; // UI의 현재 상태를 추적합니다.

    private void Start()
    {
        //recordUI.SetActive(false);
    }



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            // E 키를 누르면 UI를 켜기 or 끄기
            isUIVisible = !isUIVisible;
            recordUI.SetActive(isUIVisible);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {           
            // ESC 키를 누르면 UI 끄기
            if (isUIVisible)
            {
                recordUI.SetActive(false);
                isUIVisible = false;
            }
        }
    }
}
