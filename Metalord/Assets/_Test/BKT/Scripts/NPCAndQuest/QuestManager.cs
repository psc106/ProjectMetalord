using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // 싱글톤 패턴
    private static QuestManager instance;

    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestManager>();

                if (instance == null)
                {
                    GameObject questManager = new GameObject("QuestManager");
                    instance = questManager.AddComponent<QuestManager>();
                }
            }
            return instance;
        }
    }

    public int questId;
    public int questActionIndex;
    private List<NPC> npcs;

    Dictionary<int, QuestData> mainQuestList;
    Dictionary<int, QuestData> subQuestList;

    void Awake()
    {
        mainQuestList = new Dictionary<int, QuestData>();
        subQuestList = new Dictionary<int, QuestData>();
        GenerateData();

        NPC[] startNPC = FindObjectsOfType<NPC>();

        foreach(NPC npc in startNPC)
        {
            npcs.Add(npc);
        }
    }

    /// <summary>
    /// 퀘스트 내용 삽입 함수
    /// </summary>
    void GenerateData()
    {
        mainQuestList.Add(10, new QuestData("1번 퀘스트", new int[] { 1000, 2000 }));
        mainQuestList.Add(20, new QuestData("2번 퀘스트", new int[] { 5000, 2000 }));

        subQuestList.Add(30, new QuestData("1번 서브퀘스트", new int[] { 1000, 2000 }));
    }

    /// <summary>
    /// 퀘스트 분배 함수
    /// </summary>
    void DestributeQuest()
    {

    }

}
