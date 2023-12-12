using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int id;

    [SerializeField]
    private float interectRangeRadius = 3f; // 상호작용 범위
    private SphereCollider npcCol_Interect; // 상호작용을 위한 npc콜라이더

    private void Awake()
    {
        npcCol_Interect = GetComponent<SphereCollider>();
        SetInterectRange();
    }

    /// <summary>
    /// 게임 시작시 NPC 상호작용 범위 설정 (Trigger를 이용하여 자동 퀘스트 수락을 위한 범위)
    /// </summary>
    private void SetInterectRange()
    {
        npcCol_Interect.radius = interectRangeRadius;
    }

    /// <summary>
    /// 플레이어가 NPC에게 상호작용할때 호출하는 함수
    /// 플레이어는 NPC의 InterectNPC 함수만 호출하면 됨.
    /// NPC의 타입별로 InterectNPC함수가 호출됬을때 행동하는것은 내부에 구현예정
    /// </summary>
    public virtual void InterectNPC()
    {
        Debug.Log(gameObject.name);
    }
}
