using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedSettingsData
{
    int display_ScreenMode; // 전체(0),창(1) 모드
    int display_Resolution; // 해상도
    int display_Brightness; // 밝기
    int size_Text;          // 글자 크기
    int size_Crosshair;     // 조준선 크기
    int sound_Overall;      // 전체 오디오 크기
    int sound_Background;   // 배경 오디오 크기
    int sound_Effect;       // 효과 오디오 크기
    List<int> key_configuration;  // 키 설정값
}
