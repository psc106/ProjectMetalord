using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedGamePlayData
{
    // 획득한 수집아이템 리스트의 ID값
    List<int> obtainedItemIDList;

    

    // 소지한 재화
    int Money;

    //[임시] 맵에 배치된 재화 활성화 비활성화여부 활성화(1), 비활성화(0)
    int[] Object_Coin;

    //[임시] 맵에 배치된 오브젝트(소품) 위치값 저장을 위한 변수
    // 배열 Index번호에 맞춰서 ID값과 X,Y,Z값을 불러옴
    int[] Object_ItemId;
    int[] Object_ItemX;
    int[] Object_ItemY;
    int[] Object_ItemZ;

    //[임시] 발사되어 이용되고있는 접착제의 위치값 저장을 위한 변수
    int[] glueX;
    int[] glueY;
    int[] glueZ;

    //[임시] 활성화된 기능을 위한 변수
    bool skill_Catch;  //잡기형 스킬
    bool skill_Attach; // 부착형 스킬

    //[임시] 업그레이드된 범위 및 총알 수량을 위한 변수
    int upgrade_Amount;
    int upgrade_Range;

    // [임시] 플레이어 위치값 저장을 위한 변수
    int playerX;
    int playerY;
    int playerZ;

}
