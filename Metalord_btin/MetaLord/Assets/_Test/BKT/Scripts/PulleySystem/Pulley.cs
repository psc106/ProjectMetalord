
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour
{
    public float moveSpeed = 3f; // 도르래 이동 속도
    public Transform topPosition; // 도르래가 이동할 최상단 위치 Transform
    public Transform bottomPosition; // 도르래가 이동할 최하단 위치 Transform
    public bool isActivated = false; // 도르래가 활성화되었는지 여부


    public float divideNum = 5; // 도르래 몇번 눌렀을때 도착하게 할지
    private float upSize; // 한번 버튼을 밟았을때 올라가는 크기
    private float checkSize; // 다음 위치값 계산을 위한 변수

    private void Start()
    {
        // 한번 올라갈 크기(upSize)를 계산하여 초기화
        upSize = (topPosition.position.y - bottomPosition.position.y) / divideNum; 
    }


    void Update()
    {
        if (isActivated) // 도르래 활성화 되었는가
        {
            UpPulley(); // 활성화시 올라간다.
        }
        else
        {
            DownPulley(); // 아니면 내려간다.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            other.transform.parent = transform;
            other.transform.position = new Vector3(transform.position.x, transform.position.y - 1,transform.position.z);
            other.GetComponent<Rigidbody>().isKinematic = true;
        }
    }


    /// <summary>
    /// 다음 위치값 계산 함수
    /// 배경택_231117
    /// </summary>
    public void plusCheckSize()
    {
        checkSize = transform.position.y + upSize;
    }

    /// <summary>
    /// 도르래를 올리는 함수
    /// 배경택_231117
    /// </summary>
    private void UpPulley()
    {
        // 계산된 크기보다 현재 y위치가 낮고, 가장 높은 포지션의 y값보다 현재 y위치값이 낮으면 활성화
        if(checkSize > transform.position.y && topPosition.position.y > transform.position.y)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 도르래를 내리는 함수
    /// 배경택_231117
    /// </summary>
    private void DownPulley()
    {
        // 가장 낮은 포지션의 y값보다 현재 y위치값이 높으면 활성화
        if(bottomPosition.position.y < transform.position.y)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
    }
}
