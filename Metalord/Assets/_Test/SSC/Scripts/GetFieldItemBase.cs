using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetFieldItemBase : MonoBehaviour, IPointerDownHandler
{
    public string Id { get; protected set; }

    GetItem getItem;

    void Awake()
    {
        getItem = FindAnyObjectByType<GetItem>();
    }

    public void GetItem()
    {
        getItem.ItemGet(Id);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //GetItem();
        Debug.Log($"이 아이템의 아이디는 : {Id}");
    }

}
