using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오디오 매니저
/// 231215 배경택
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("오디오 소스")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("오디오 클립")]
    [Tooltip("필요한 오디오 클립을 인스펙터창에서 넣어주면 됩니다.")]
    public AudioClip clip_Background;
    public AudioClip clip_Getcoin;
    public AudioClip clip_Jump;

    private void Start()
    {
        PlayBackGroundMusic(); // 최초 음악시작 -> 음악시작위치 조정 필요시 위치 변경
    }

    // 최초 음악 시작 시점
    public void PlayBackGroundMusic()
    {
        if(clip_Background != null)
        {
            musicSource.clip = clip_Background;
            musicSource.Play();
        }
        else Debug.Log("오디오 파일이 없습니다.");
    }

    // 배경 음악 변경시
    public void ChangeBackGroundMusic(AudioClip clip)
    {
        if(clip != null) musicSource.clip = clip;
        else Debug.Log("오디오 파일이 없습니다.");
    }

    // 효과음 실행시
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) SFXSource.PlayOneShot(clip);
        else Debug.Log("오디오 파일이 없습니다.");
    }


}
