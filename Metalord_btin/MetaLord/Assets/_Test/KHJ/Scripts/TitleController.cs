using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameSceneController;

public class TitleController : MonoBehaviour
{
    SceneList currentScene;
    SceneList nextScene;
    SceneList continueScene;

    private void Awake()
    {
        currentScene = SceneList.KHJ_TestTitleScene1;
        nextScene = SceneList.StoryScene;
        continueScene = SceneList.MainScene;
    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneAsync(currentScene, nextScene));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneAsync(currentScene, continueScene));
    }
}
