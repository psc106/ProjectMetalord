using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string questName;
    public int[] npcId;

    public QuestData(string _questName, int[] _npcId)
    {
        questName = _questName;
        npcId = _npcId;
    }
}
