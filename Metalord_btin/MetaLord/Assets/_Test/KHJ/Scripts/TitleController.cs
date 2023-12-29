using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameSceneController;

public class TitleController : MonoBehaviour
{
    SceneList currentScene;
    SceneList nextScene;

    private void Awake()
    {
        currentScene = SceneList.KHJ_TestTitleScene1;
        nextScene = SceneList.StoryScene;
    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneAsync(currentScene, nextScene));
    }
}
