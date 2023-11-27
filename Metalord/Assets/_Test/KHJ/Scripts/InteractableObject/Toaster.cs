using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Toaster : MonoBehaviour
{
    private GameObject firstToast; //올라갈 빵 계단 오브젝트 
    private Vector3 firstToastPos; // 올라갈 빵 처음 위치
    

    private GameObject secondToast; //올라갈 빵 계단 오브젝트 
    private Vector3 secondToastPos; // 올라갈 빵 처음 위치
   


    public bool isToaster; //계단 올리기 위한 트리거 
    private bool isTriggerOn; // isToaster 확인하기 위한 bool 값

    private void Start()
    {
        firstToast = transform.GetChild(0).gameObject;
        firstToastPos = firstToast.transform.localPosition;
       
        secondToast = transform.GetChild(1).gameObject;
        secondToastPos = secondToast.transform.localPosition;

        isToaster = false;
        isTriggerOn = false;
    }

    //TODO : Lerp 를 사용해서 코루틴으로 빵이 이동하는 것을 구현해야함 
    public void UpToast(Transform upObject, float heighFloat)
    {
        //upObject.localPosition = new Vector3(upObject.localPosition.x, upObject.localPosition.y * heighFloat, upObject.localPosition.z);

        if(isToaster == false)
        {
            StartCoroutine(UpToastObject(firstToast.transform, 2f));
            StartCoroutine(UpToastObject(secondToast.transform, 3f));
        }
    }

    public void UpToast()
    {
        if (isToaster == false)
        {
            StartCoroutine(UpToastObject(firstToast.transform, 2f));
            StartCoroutine(UpToastObject(secondToast.transform, 3f));
        }
    }

    private void DownToast(Transform upObject, Vector3 targetPos)
    {
        upObject.localPosition = targetPos;
    }
    public void DownToast()
    {
        if(isToaster == true)
        {
            StartCoroutine(DownToastObject(firstToast.transform, firstToastPos));
            StartCoroutine(DownToastObject(secondToast.transform, secondToastPos));
        }
    }

    IEnumerator UpToastObject(Transform upObject, float heighFloat)
    {
        float duration = 0;
        float moveSpeed = 2f;
        Vector3 startPos = upObject.localPosition;
        Vector3 endPos = new Vector3(upObject.localPosition.x, upObject.localPosition.y * heighFloat, upObject.localPosition.z);
        while (duration <= 1f)
        {
            //upObject.position = Vector3.Lerp(upObject.localPosition , new Vector3(upObject.localPosition.x, upObject.localPosition.y * heighFloat, upObject.localPosition.z), duration);
            upObject.localPosition  = Vector3.Lerp(startPos, endPos, duration);
            if (startPos.y >= endPos.y)
            {
                startPos.y = endPos.y;
                yield break;
            }
            duration += moveSpeed * Time.deltaTime;
            yield return null;
        }

        isToaster = true;
    }

    IEnumerator DownToastObject(Transform originTransform, Vector3 targetPos)
    {
        float duration = 0;
        float moveSpeed = 2f;
        Vector3 startPos = originTransform.localPosition;
        Vector3 endPos = targetPos;
        while (duration <= 1f)
        {
            //upObject.position = Vector3.Lerp(upObject.localPosition , new Vector3(upObject.localPosition.x, upObject.localPosition.y * heighFloat, upObject.localPosition.z), duration);
            originTransform.localPosition = Vector3.Lerp(startPos, endPos, duration);
            duration += moveSpeed * Time.deltaTime;
            yield return null;
        }
        isToaster = false;
        
    }
}
