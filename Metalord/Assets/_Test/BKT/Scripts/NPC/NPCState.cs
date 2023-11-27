using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC의 상태표시
/// </summary>
public enum NPCState
{
    REQUIREMENTS_NOT_MET, // 요구사항이 충족되지 않아, 기본적인 스크립트만 반복해서 말하는 상태
    CAN_START, // 이제 말을 걸 경우, 정해진 행동(카메라 줌인 or 스크립트 or 이동)이 가능한 상태
    IN_PROGRESS, // 정해진 행동을 진행중인 상태
    FINISHED // NPC 역할을 끝내서, 반복적인 행동을 하거나, 추후 마지막 사진 중첩에서 사용될 상태
}
