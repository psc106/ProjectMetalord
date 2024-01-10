using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayTest : MonoBehaviour
{
    [SerializeField] private GameObject videoPlayer;
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SFXSource;

    float save_BGMVolume;
    float save_SFXVolume;

    public void Start()
    {
        save_BGMVolume = BGMSource.volume;
        save_SFXVolume = SFXSource.volume;
        BGMSource.volume = 0.3f;
        SFXSource.volume = 0.3f;
    }

    public void TestPlayVideo()
    {
        videoPlayer.SetActive(true);
        BGMSource.mute = true;
        SFXSource.mute = true;

    }

    public void OffCanvas()
    {
       StartCoroutine(OffGame());

    }

    IEnumerator OffGame() 
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        BGMSource.mute = false;
        SFXSource.mute = false;
        BGMSource.volume = save_BGMVolume;
        SFXSource.volume = save_SFXVolume;
    }
}
