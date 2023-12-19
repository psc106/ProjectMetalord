using System;

/// <summary>
/// 유틸리티 클래스
/// 231219 배경택
/// </summary>
public static class Utility
{
    // 도감 아이템을 Enum순서대로 가져오기 위한 배열
    private static RecordList[] recordListArr =
        {
            RecordList.FrenchFries,
            RecordList.FishBones,
            RecordList.BrokenPottery,
            RecordList.WineGlass,
            RecordList.ChineseKnife,
            RecordList.ForkSpoonSet,
            RecordList.CupCake,
            RecordList.HalloweenPumpkin,
            RecordList.AcousticGuitar,
            RecordList.CandyCane,
            RecordList.ToyShovel,
            RecordList.RecordCoin,
            RecordList.WoodenTrain,
            RecordList.BlueStar,
            RecordList.RemoteControl,
            RecordList.Cube,
            RecordList.WoodenShield,
            RecordList.GoldenChessPiece,
            RecordList.LightSaber,
            RecordList.TechDeck,
        };

    // 외부에서 아이템 호출시
    public static int GetRecordId(int index)
    {
        return (int)recordListArr[index];
    }
}

/// <summary>
/// 구역 Enum
/// </summary>
public enum Zone
{
    Total = 0,
    Kitchen = 1,
    LivingRoom = 2,
    BabyRoom = 3
}

/// <summary>
/// 도감 아이템 Enum
/// </summary>
public enum RecordList
{
    FrenchFries = 100,
    FishBones = 101,
    BrokenPottery = 102,
    WineGlass = 103,
    ChineseKnife = 104,
    ForkSpoonSet = 105,
    CupCake = 106,
    HalloweenPumpkin = 200,
    AcousticGuitar = 201,
    CandyCane = 202,
    ToyShovel = 203,
    RecordCoin = 204,
    WoodenTrain = 205,
    BlueStar = 300,
    RemoteControl = 301,
    Cube = 302,
    WoodenShield = 303,
    GoldenChessPiece = 304, 
    LightSaber = 305,
    TechDeck = 306,
}