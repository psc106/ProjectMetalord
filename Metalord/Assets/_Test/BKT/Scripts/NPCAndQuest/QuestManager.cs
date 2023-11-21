using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    // 퀘스트 매니저 활성화시 이벤트 추가
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    }

    // 퀘스트 매니저 비활성화시 이벤트 제거
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;
    }

    //시작시 
    private void Start()
    {

        foreach (Quest quest in questMap.Values)
        {
            // initialize any loaded quest steps
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // broadcast the initial state of all quests on startup
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }


    // 퀘스트 요구사항 충족시켰는지 체크하는 함수
    // 요구사항은 매개변수의 quest에 들어있음
    private bool CheckRequirementsMet(Quest quest)
    {
        // 시작시 요구사항 충족으로 시작,(true) 뒤에 판별을 통해 false or true 반환
        bool meetsRequirements = true;

        // 플레이어 레벨조건 확인이지만, 메타로드에서는 사용안함
        //if (currentPlayerLevel < quest.info.levelRequirement)
        //{
        //    meetsRequirements = false;
        //}

        // quest의 전제조건들이 전부 클리어되었는지를 체크함
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        // loop through ALL quests
        foreach (Quest quest in questMap.Values)
        {
            // if we're now meeting the requirements, switch over to the CAN_START state
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        // move on to the next step
        quest.MoveToNextStep();

        // if there are more steps, instantiate the next one
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        // if there are no more steps, then we've finished all of them for this quest
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }


    // 보상을 청구하는 함수
    private void ClaimRewards(Quest quest)
    {
        // TODO 보상은 여기에서 처리

        //GameEventsManager.instance.goldEvents.GoldGained(quest.info.goldReward);
        //GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // Assets/Resources/Quests 폴더 아래에 있는 모든 QuestInfoSO 스크립터블 오브젝트를 로드합니다.
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        // 퀘스트 맵을 생성합니다.
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        // 모든 퀘스트 정보를 반복하여 맵에 추가합니다.
        foreach (QuestInfoSO questInfo in allQuests)
        {
            // 이미 같은 ID를 가진 퀘스트가 맵에 있는지 확인합니다.
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                // 중복된 ID를 찾았을 때 경고를 출력합니다.
                Debug.LogWarning("퀘스트 맵 생성 중 중복된 ID 발견: " + questInfo.id);
            }

            // 각 퀘스트를 로드하여 맵에 추가합니다.
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }

        return idToQuestMap;
    }


    // ID를 통해 Quest를 불러옴
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }


    // 어플 종료시 퀘스트를 저장함
    private void OnApplicationQuit()
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }


    // 퀘스트를 저장함
    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            // JsonUtility를 사용하여 직렬화하지만, 필요에 따라 JSON.NET 등 다른 방법을 사용할 수 있습니다.
            string serializedData = JsonUtility.ToJson(questData);
            // PlayerPrefs에 저장하는 것은 튜토리얼 비디오에서 빠르게 설명하기 위한 예시일 뿐입니다.
            // 실제로는 장기적으로 이 정보를 거기에 저장하길 원하지 않을 것입니다.
            // 대신 실제로 사용할 수 있는 저장 및 불러오기 시스템을 사용하여 파일, 클라우드 등에 저장하는 것이 좋습니다.
            PlayerPrefs.SetString(quest.info.id, serializedData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("퀘스트 ID " + quest.info.id + "를 저장하는 데 실패했습니다: " + e);
        }
    }

    //퀘스트 불러오기
    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;
        try
        {
            // load quest from saved data
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            // otherwise, initialize a new quest
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }
}
