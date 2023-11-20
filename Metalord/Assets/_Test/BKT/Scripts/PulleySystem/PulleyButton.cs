
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PulleyButton : MonoBehaviour
{
    public Pulley pulley;

    [SerializeField]
    private float limitTime = 10f;

    private float initTime = 0f;

    private void Update()
    {
        initTime += Time.deltaTime;
        if(initTime > limitTime) // 제한시간이 다 되었다면
        {
            PulleyActiveFalse(); // 도르래 비활성화
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어가 버튼을 누르면 도르래가 올라감
        if (collision.transform.tag == "Player")
        {
            pulley.plusCheckSize(); // 다음 올라갈 위치 계산
            pulley.isActivated = true; // 도르래 활성화
            initTime = 0f; //초기화 시간 0으로 초기화
        }
    }

    /// <summary>
    /// pulley 비활성화를 위한 함수
    /// </summary>
    private void PulleyActiveFalse()
    {
        pulley.isActivated = false; // 도르래 비활성화
    }

    

}
