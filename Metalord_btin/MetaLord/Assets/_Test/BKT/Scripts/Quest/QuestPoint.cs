using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CircleCollider2D 구성 요소가 필요합니다.
[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("퀘스트")]
    // QuestInfoSO 스크립터블 오브젝트를 직렬화합니다.
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("설정")]
    // 시작 지점인지 여부를 결정합니다.
    [SerializeField] private bool startPoint = true;
    // 완료 지점인지 여부를 결정합니다.
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false; // 플레이어가 근처에 있는지 여부를 확인합니다.
    private string questId; // 퀘스트 ID를 저장합니다.
    private QuestState currentQuestState; // 현재 퀘스트 상태를 저장합니다.

    private QuestIcon questIcon; // 퀘스트 아이콘을 관리합니다.

    private void Awake()
    {
        questId = questInfoForPoint.id; // 퀘스트 ID를 설정합니다.
        questIcon = GetComponentInChildren<QuestIcon>(); // 자식 오브젝트에서 퀘스트 아이콘을 가져옵니다.
        //Debug.Log(questInfoForPoint);
    }

    private void OnEnable()
    {
        // 퀘스트 상태 변경 이벤트를 수신합니다.
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        // 제출 버튼 누름 이벤트를 수신합니다.
        // GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        // 퀘스트 상태 변경 이벤트 구독을 해제합니다.
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        // 제출 버튼 누름 이벤트 구독을 해제합니다.
        // GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return; // 플레이어가 근처에 없으면 함수를 종료합니다.
        }

        // 퀘스트를 시작하거나 완료합니다.
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            GameEventsManager.instance.questEvents.StartQuest(questId); // 퀘스트 시작 이벤트를 호출합니다.
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId); // 퀘스트 완료 이벤트를 호출합니다.
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // 해당 포인트가 해당 퀘스트를 가지고 있는 경우에만 퀘스트 상태를 업데이트합니다.
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state; // 현재 퀘스트 상태를 업데이트합니다.
            questIcon.SetState(currentQuestState, startPoint, finishPoint); // 퀘스트 아이콘 상태를 설정합니다.
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true; // 플레이어가 충돌 범위 안으로 들어왔습니다.
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false; // 플레이어가 충돌 범위에서 나갔습니다.
        }
    }
}
