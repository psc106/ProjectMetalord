using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedGamePlayData
{
    // 획득한 수집아이템 리스트의 ID값
    public List<int> obtainedItemIDList;

    // 소지한 재화
    public int Money;

    //[임시] 맵에 배치된 오브젝트(소품) 위치값 저장을 위한 변수
    // 배열 Index번호에 맞춰서 ID값과 X,Y,Z값을 불러옴
    public string[] recordItemTransform = new string[100];

    // 1~50 작은코인, 51~99 큰코인, 100~ 주방,200~ 거실, 300~ 아기방
    public int[] coinAndRecordItem = new int[400];


    //[임시] 활성화된 기능을 위한 변수
    public bool skill_Catch;  //잡기형 스킬

    //[임시] 업그레이드된 범위 및 총알 수량을 위한 변수
    public int upgrade_Amount;
    public int upgrade_Range;

    // [임시] 플레이어 위치값 저장을 위한 변수
    public string playerTransform;
}
