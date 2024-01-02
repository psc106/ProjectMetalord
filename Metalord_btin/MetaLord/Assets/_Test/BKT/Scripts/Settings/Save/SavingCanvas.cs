using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavingCanvas : MonoBehaviour
{
    public bool isSaved = false;

    private void OnDisable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        isSaved = false;
    }

    public void StartSave()
    {
        gameObject.SetActive(true);
        Controller_Physics.SwitchCameraLock(false);
        StartCoroutine(PopSavingUI());
    }

    // 저장 UI창 띄우기
    IEnumerator PopSavingUI()
    {
        string dot = default;
        for (int i = 0; i < 3; i++)
        {
            dot += ".";
            Debug.Log(dot);
            transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = dot;
            yield return new WaitForSecondsRealtime(1f);
            Debug.Log("코루틴");
        }
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        isSaved = true;
    }
}
