using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 메인씬에서 한번에 MoveObject에 id값을 입력해주는 함수
/// </summary>
public class MainSceneManager : MonoBehaviour
{
    static public MainSceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;            
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        InputIdToObject();
    }

    public void InputIdToObject()
    {
        MoveObjectData[] moveObjects = FindObjectsByType<MoveObjectData>(FindObjectsSortMode.InstanceID);
        Coin[] coinObjects = FindObjectsByType<Coin>(FindObjectsSortMode.InstanceID);

        for(int i = 0; i < moveObjects.Length; i++)
        {
            moveObjects[i].id = i;
            //Debug.Log(i);
        }

        for(int i = 0; i < coinObjects.Length; i++)
        {
            //Debug.Log(coinObjects[i].name);
            coinObjects[i].id = i;
        }
    }
}
