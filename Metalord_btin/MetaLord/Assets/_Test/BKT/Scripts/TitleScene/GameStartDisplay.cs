using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartDisplay : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("BKT_TestScene");
    }

    public void LoadGame()
    {
        //TODO 저장정보 불러오기 함수

        Debug.Log("아직 저장 정보가 없습니다.");
    }
}
