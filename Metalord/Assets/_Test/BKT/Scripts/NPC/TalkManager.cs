using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        InitData();
    }

    private void InitData()
    {
        talkData.Add(1000, new string[] { "NPC A입니다.", "말 걸지 말아주세요" });
        talkData.Add(2000, new string[] { "NPC B입니다.", "저는 말입니다" });

    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];
    }
}
