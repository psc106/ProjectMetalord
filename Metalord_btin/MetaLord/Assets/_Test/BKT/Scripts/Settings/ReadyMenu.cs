using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대기화면 메뉴
/// 231226 배경택
/// </summary>
public class ReadyMenu : MonoBehaviour
{

    [SerializeField] GameObject gameExplainCanvas;
    [SerializeField] GameObject SettingsCanvas;

    // 돌아가기 버튼
    public void BackToGame()
    {
        gameObject.SetActive(false);
        CanUseSound();
    }

    // 도움말 버튼
    public void GameExplain()
    {
        gameExplainCanvas.SetActive(true);
        gameObject.SetActive(false);
        CanUseSound();

    }

    // 환경설정창으로 이동
    public void GoSettings()
    {
        SettingsCanvas.SetActive(true);
        gameObject.SetActive(false);
        CanUseSound();
    }

    // 게임 저장
    public void GameSave()
    {
        GameEventsManager.instance.dataEvents.SaveData(); // 저장 이벤트 발생
    }

    // 임시 버튼 사용 불가 소리
    public void CantUseSound()
    {
        // 사운드 추가
        SoundManager.instance.PlaySound(GroupList.UI, (int)UISoundList.Cant_BuySound);
    }

    // 임시 버튼 사용가능소리
    private void CanUseSound()
    {
        // 사운드 추가       
        SoundManager.instance.PlaySound(GroupList.UI, (int)UISoundList.Can_BuySound);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
