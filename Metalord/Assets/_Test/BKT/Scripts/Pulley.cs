
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour
{
    public float moveSpeed = 3f; // 풀리 이동 속도
    public Transform topPosition; // 풀리가 이동할 최상단 위치 Transform
    public Transform bottomPosition; // 풀리가 이동할 최하단 위치 Transform

    public bool isActivated = false; // 풀리가 활성화되었는지 여부

    void Update()
    {
        if (isActivated)
        {
            UpPulley();
        }
        else
        {
            DownPulley();
        }
    }


    //public void UpPulley()
    //{
    //    if (transform.position.y < topPosition.position.y) // 최상단 위치보다 위에 있지 않으면 위로 이동
    //    {
    //        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    //    }
    //    else // 최상단 위치에 도달하면 비활성화
    //    {
    //        isActivated = false;
    //    }

    //    if (transform.position.y > bottomPosition.position.y) // 최하단 위치보다 아래에 있지 않으면 아래로 이동
    //    {
    //        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    //    }
    //    else // 최하단 위치에 도달하면 비활성화
    //    {
    //        isActivated = false;
    //    }
    //}

    private void UpPulley()
    {
        if (bottomPosition)
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }


    private void DownPulley()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}
