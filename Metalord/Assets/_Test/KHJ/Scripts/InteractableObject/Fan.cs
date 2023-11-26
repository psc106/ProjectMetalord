using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour, IInteractableObject
{
    private bool isFan = false;
    public float fanForce = 0f;

    public BoxCollider fanRangeCollider;
    private Vector3 colliderCenter;
    private Vector3 colliderSize;
    private float zHalfSize;

    private Vector3 zMinPoint;
    private Vector3 zMaxPoint;

    private void Start()
    {
        colliderCenter = fanRangeCollider.center; // 박스 콜라이더의 중심점
        colliderSize = fanRangeCollider.size; // 박스 콜라이더의 크기
        zHalfSize = colliderSize.z * 0.5f; // Z 축 크기의 절반
        zMinPoint = transform.TransformPoint(colliderCenter - new Vector3(0, 0, zHalfSize)); // Z 최소값 좌표
        zMaxPoint = transform.TransformPoint(colliderCenter + new Vector3(0, 0, zHalfSize)); // Z 최대값 좌표
        fanForce = 10f; //밀어내는 힘
    }

    public void Interact()
    {
        Debug.Log("선풍기 Interact 메서드 실행됨");
        isFan = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && isFan == true)
        {
            Rigidbody objRb = other.transform.GetComponent<Rigidbody>(); //닿는 오브젝트 rigid 컴포넌트 가져오기
            Vector3 fanDirection = transform.forward; //밀어내는 방향 값 저장
            Vector3 objPosition = other.transform.position; // 닿는 오브젝트 포지션 저장
            float distance = Mathf.Clamp01((objPosition.z - zMinPoint.z) / (zMaxPoint.z - zMinPoint.z)); // 위치 정규화
            if((1f - distance) > 0.8f) 
            {
                fanForce = 20f;
            }
            else
            {
                fanForce = 10f;
            }
            float calculatedForce = fanForce * (1f - distance); //거리에 따라 밀어내는 힘 계산
            Debug.LogFormat("밀어내는 힘 = {0}", calculatedForce);
            objRb.useGravity = false; //일단 이걸 끄면 적은 힘으로도 쉽게 밀어낼 수 있어서 일단 끔
            objRb.AddForce(fanDirection * calculatedForce, ForceMode.Force);
        }
        //TODO 처음 가까운 거리에서 멀어졌을때 addforce 를 감쇠해서 자연스럽게 밀려나게끔 바꿔줘야함
        //현재는 AddForce가 누적되어서 멀리 있는데도 빨리 밀려나는 문제가 존재.
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isFan == true)
        {
            Rigidbody objRb = other.transform.GetComponent<Rigidbody>();
            objRb.useGravity = true;
            objRb.velocity = Vector3.zero;
        }
    }
}
