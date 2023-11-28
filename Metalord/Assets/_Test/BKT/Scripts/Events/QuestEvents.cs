using System;

public class QuestEvents
{
    // 퀘스트 시작시 호출되는 이벤트
    public event Action<string> onStartQuest;
    public void StartQuest(string id)
    {
        // onStartQuest가 비어있지 않다면
        if (onStartQuest != null)
        {
            // 퀘스트 id에 맞는 퀘스트를 실행시킴
            onStartQuest(id);
        }
    }

    // 퀘스트 진행중 호출되는 이벤트
    public event Action<string> onAdvanceQuest;
    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public event Action<string> onFinishQuest;
    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }

    public event Action<string, int, QuestStepState> onQuestStepStateChange;
    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        if (onQuestStepStateChange != null)
        {
            onQuestStepStateChange(id, stepIndex, questStepState);
        }
    }
}
