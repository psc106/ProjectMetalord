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
        float duration = Time.deltaTime * 10f;
        //for (float i = transform.localScale.y; i >= downSize.y; i--)
        //{

        //    //else if ( isPlay  == true) 
        //    //{
        //    //    yield break;
        //    //}

        //    transform.localScale = Vector3.Lerp(transform.localScale, downSize, duration);
        //    yield return null;
            
        //}
        while ( transform.localScale.y > downSize.y ) 
        {            
            //Debug.Log("트램펄린 업 실행됨?");
            transform.localScale = Vector3.Lerp(transform.localScale, downSize, duration);
            yield return null;
            Debug.Log("여기 ㅇ무슨 ㅅ문데");
            if (transform.localScale.y <= downSize.y)
            {
                Debug.Log("브레이크");
                transform.localScale = downSize;
                isPlay = true;
                yield break;
            }

        }
    }


    IEnumerator ScaleUp()
    {
        float duration = Time.deltaTime;
        //for(float i = downSize.y; i <= originSize.y; i++)
        //{
        //    Debug.Log("트램펄린 업 실행됨?");
        //    transform.localScale = Vector3.Lerp(transform.localScale, originSize, duration);
        //    yield return null;
        //}
        while (transform.localScale.y <= originSize.y)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originSize, duration);
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
