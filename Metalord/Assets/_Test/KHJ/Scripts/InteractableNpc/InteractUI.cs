using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    public GameObject interactPanel = default;
    //private TMP_Text interactText = default;
    public bool isOnPanel = false;

    private void Awake()
    {
        //interactText = interactPanel.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void Start()
    {
        isOnPanel = false;
        interactPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPanel.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPanel.gameObject.SetActive(false);
        }
    }


    //public void ControlInteractPanel()
    //{
    //    if (isOnPanel)
    //    {
    //        interactPanel.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.Log("들어옴?");
    //        interactPanel.SetActive(false);
    //    }
    //}

    //public void SetText(string detail)
    //{
    //    interactText.text = string.Format("{0}와 대화하기", detail);
    //}
}
