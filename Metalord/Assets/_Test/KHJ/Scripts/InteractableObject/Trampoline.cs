using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

    private Vector3 originSize = default;
    public Vector3 downSize = default;

    public bool isPlay = false;

    private void Start()
    {
        isPlay = false;
        originSize = transform.localScale;
        downSize = new Vector3(transform.localScale.x, transform.localScale.y - 2f, transform.localScale.z);
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            CapsuleCollider capsuleCollider = other.gameObject.GetComponent<CapsuleCollider>();
            PhysicMaterial otherPhysicMat = capsuleCollider.material;
            otherPhysicMat.bounciness = 0.9f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CapsuleCollider capsuleCollider = other.gameObject.GetComponent<CapsuleCollider>();
            PhysicMaterial otherPhysicMat = capsuleCollider.material;
            otherPhysicMat.bounciness = 0f;
        }
    }

    IEnumerator ScaleDown()
    {
        //float duration = 1f;

        //for (float i = transform.localScale.y; i >= downSize.y; i--)
        //{

        //    //else if ( isPlay  == true) 
        //    //{
        //    //    yield break;
        //    //}

        //    transform.localScale = Vector3.Lerp(transform.localScale, downSize, duration);
        //    yield return null;

        //}

        //while ( transform.localScale.y > downSize.y ) 
        //{
        //    //transform.localScale = Vector3.Lerp(transform.localScale, downSize, duration);
        //    //이런 식으로  Lerp 함수를 사용하면 매번 새로운 스케일 값을 설정하면서 while 루프를 실행할 때 문제가 발생합니다.
        //    //{변경 후
        //    float lerp = Mathf.Lerp(transform.localScale.y, downSize.y, duration);
        //    transform.localScale = new Vector3(transform.localScale.x, lerp, transform.localScale.z);
        //    //}변경 후
        //    yield return null;
        //}

        //=================================

        float duration = 1f;
        float elapsedTime = 0.9f;

        while (transform.localScale.y > downSize.y)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 시간 비율 계산

            // 보간된 값을 직접 계산하여 스케일 조정
            float newYScale = Mathf.Lerp(originSize.y, downSize.y, t);
            transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);

            yield return null;
        }


        isPlay = true;
        
        if (transform.localScale.y <= downSize.y)
        {
            transform.localScale = downSize;
            yield break;
        }
    }


    IEnumerator ScaleUp()
    {
        float duration = 1f;
        //for(float i = downSize.y; i <= originSize.y; i++)
        //{
        //    Debug.Log("트램펄린 업 실행됨?");
        //    transform.localScale = Vector3.Lerp(transform.localScale, originSize, duration);
        //    yield return null;
        //} 맨처음 했던 방식 이떄는 lerp를 써서 스케일을 다시 해도 for문으로 횟수를 정해줬던 거라 문제가 없었다. 아니 있었나?

        //while (transform.localScale.y <= originSize.y)
        //{
        //    //{변경 전
        //    //transform.localScale = Vector3.Lerp(transform.localScale, originSize, duration);
        //    //}변경 전

        //    //{변경 후
        //    float lerp = Mathf.Lerp(transform.localScale.y, originSize.y, duration);
        //    transform.localScale = new Vector3 (transform.localScale.x,lerp, transform.localScale.z);   
        //    //}변경 후

        //    yield return null;

        //}
        //===========================================

        float elapsedTime = 0.7f;

        while (transform.localScale.y < originSize.y)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 시간 비율 계산

            // 보간된 값을 직접 계산하여 스케일 조정
            float newYScale = Mathf.Lerp(downSize.y, originSize.y, t);
            transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);

            yield return null;
        }

        isPlay = false;
        if (transform.localScale.y >= originSize.y)
        {
           
            transform.localScale = originSize;
            yield break;
        }

    }


    public void TouchPad()
    {
        Debug.Log(isPlay);
        if (isPlay == false)
        {
            StartCoroutine(ScaleDown());
        }
    }

    public void ChangeOriginSize()
    {
        Debug.Log(isPlay);

        if (isPlay == true)
        {
            StartCoroutine(ScaleUp());
        }
    }
}
