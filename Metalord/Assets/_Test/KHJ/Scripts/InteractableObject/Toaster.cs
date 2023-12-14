using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Toaster : MonoBehaviour
{
    private float goalHeigh = 0f;
    private GameObject firstToast; //올라갈 빵 계단 오브젝트 
    private Vector3 firstToastPos; // 올라갈 빵 처음 위치
    public Vector3 firstToastGoal;

    private GameObject secondToast; //올라갈 빵 계단 오브젝트 
    private Vector3 secondToastPos; // 올라갈 빵 처음 위치
    public Vector3 secondToastGoal;

    public bool isToaster; //계단 올리기 위한 트리거 

    private void Start()
    {
        goalHeigh = 2f;
        firstToast = transform.GetChild(0).gameObject;
        firstToastPos = firstToast.transform.localPosition;
        firstToastGoal = new  Vector3(firstToastPos.x, firstToastPos.y * goalHeigh, firstToastPos.z);

        goalHeigh = 3f;
        secondToast = transform.GetChild(1).gameObject;
        secondToastPos = secondToast.transform.localPosition;
        secondToastGoal = new Vector3(secondToastPos.x, secondToastPos.y * goalHeigh, secondToastPos.z);

        isToaster = false;
    }

    public void UpToast(Transform upObject, float heighFloat)
    {
        //upObject.localPosition = new Vector3(upObject.localPosition.x, upObject.localPosition.y * heighFloat, upObject.localPosition.z);
    }

    public void UpToast()
    {
        if (isToaster == false)
        {             
            StartCoroutine(UpToastObject(firstToast.transform,firstToastGoal, 2f));
            StartCoroutine(UpToastObject(secondToast.transform, secondToastGoal, 3f));
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
    
    IEnumerator UpToastObject(Transform upObject, Vector3 goalPos ,float heighFloat)
    {
        float duration = 0;
        float moveSpeed = 2f;
        Vector3 startPos = upObject.localPosition;
        Vector3 endPos = goalPos;

        while (duration <= 1f ) 
        {
            //upObject.position = Vector3.Lerp(upObject.localPosition , new Vector3(upObject.localPosition.x, upObject.localPosition.y * heighFloat, upObject.localPosition.z), duration);
            upObject.localPosition  = Vector3.Lerp(startPos, endPos, duration);
            
            duration += moveSpeed * Time.deltaTime;
            if (upObject.transform.localPosition.y >= endPos.y || isToaster)
            {
                
                yield break;
            }
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
       
        while (duration <= 1f )
        {
            //upObject.position = Vector3.Lerp(upObject.localPosition , new Vector3(upObject.localPosition.x, upObject.localPosition.y * heighFloat, upObject.localPosition.z), duration);
            originTransform.localPosition = Vector3.Lerp(startPos, endPos, duration);
            duration += moveSpeed * Time.deltaTime;
            if (startPos.y >= endPos.y && isToaster == false)
            {
                yield break;
            }
            yield return null;
        }
        isToaster = false;
        
    }
}
