using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StorySceneManager : MonoBehaviour
{
    [SerializeField]
    AudioSource sfxAudio;
    [SerializeField]
    AudioSource bgmAudio;
    [SerializeField]
    Image image;
    
    [SerializeField]
    Sprite[] images;
    [SerializeField]
    float[] imageTimes;
    [SerializeField]
    int footIndex = 17;
    bool canSkip = true;

    [SerializeField]
    GameSceneController.SceneList currentScene;
    [SerializeField]
    GameSceneController.SceneList nextScene;

    private void Start()
    {
        StartCoroutine(PlayStoryRoutine());
    }

    private void Update()
    {
        if (Input.anyKey && canSkip)
        {
            canSkip = false;
            StopAllCoroutines();
            LoadNextScene();
        }
    }

    IEnumerator PlayStoryRoutine()
    {
        int currIndex = 0;
        while (currIndex < images.Length)
        {
            image.sprite = images[currIndex];
            yield return new WaitForSeconds(imageTimes[currIndex] * 0.5f);
            if (footIndex == currIndex)
            {
                sfxAudio.Play();
                StartCoroutine(VolumeUpRoutine(Time.deltaTime * 2, 20));
            }
            yield return new WaitForSeconds(imageTimes[currIndex] * 0.5f);
            currIndex += 1;
        }
        bgmAudio.Stop();
        yield return new WaitForSeconds(1f);
        if (canSkip) LoadNextScene();
    }

    IEnumerator VolumeUpRoutine(float time, int count)
    {
        while (count>0)
        {
            bgmAudio.volume += Time.deltaTime;
            yield return new WaitForSeconds(time);
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(GameSceneController.LoadSceneAsync(currentScene, nextScene));
    }

}
