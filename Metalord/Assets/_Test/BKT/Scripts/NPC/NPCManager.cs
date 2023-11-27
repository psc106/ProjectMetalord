using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{

    private Dictionary<string, NPCAction> npcMap;

    private void Awake()
    {
        npcMap = CreateNPCMap();
    }

    private void Start()
    {
        foreach(NPCAction npcAction in npcMap.Values)
        {
            if(npcAction.state == NPCState.IN_PROGRESS)
            {
                
            }
        }
    }

    // 전체 NPC행동을 Dictionary에 저장
    private Dictionary<string, NPCAction> CreateNPCMap()
    {
        NPCInfo[] allNpcAction = Resources.LoadAll<NPCInfo>("NPCInfo");

        Dictionary<string, NPCAction> idToNPCMap = new Dictionary<string, NPCAction>();
        foreach(NPCInfo npcInfo in allNpcAction)
        {
            if (idToNPCMap.ContainsKey(npcInfo.id))
            {
                Debug.LogWarning("중복된 ID를 가진 NPC가 이미 있습니다." + npcInfo.id);
            }
            idToNPCMap.Add(npcInfo.id, LoadNPCInfo(npcInfo));
        }
        return idToNPCMap;
    }

    // 어플리케이션 종료시
    private void OnApplicationQuit()
    {
        //TODO NPC 행동이 어느정도까지 진행되었는지 저장하도록 구현
    }

    // NPC 행동을 생성하는 함수
    private NPCAction LoadNPCInfo(NPCInfo _npcInfo)
    {
        NPCAction npcAction = null;

        if (false)
        {
            //TODO NPC저장내용 불러오기
        }
        else
        {
            npcAction = new NPCAction(_npcInfo); // 새로 생성
        }

        return npcAction;
    }
}
