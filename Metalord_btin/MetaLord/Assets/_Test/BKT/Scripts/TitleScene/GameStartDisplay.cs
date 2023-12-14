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
}
