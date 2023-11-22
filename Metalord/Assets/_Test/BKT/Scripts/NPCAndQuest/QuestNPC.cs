using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : NPC
{
    List<QuestData> quests;

    /// <summary>
    /// NPC에게 퀘스트를 할당하는 함수
    /// </summary>
    protected void AddQuestToNPC(QuestData quest)
    {
        //이 함수를 통해 QuestNPC
        quests.Add(quest);
    }
}
