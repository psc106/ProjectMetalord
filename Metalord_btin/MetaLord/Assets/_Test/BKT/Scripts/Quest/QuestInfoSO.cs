using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName; //이름

    [Header("Requirements")] //퀘스트 시작 요구조건 
    public QuestInfoSO[] questPrerequisites; //퀘스트 전제조건

    [Header("Steps")] // 퀘스트 해결을 위한 단계_ 단계 전부 클리어시 보상
    public GameObject[] questStepPrefabs;

    //TODO 보상관련해서 기획서 정해질 시 작성
    //[Header("Rewards")]
    //public int goldReward;
    //public int experienceReward;

    // 에디터에서 ScriptableObject의 이름을 id에 할당하는 역할
    // EditorUtility를 사용하여 변경된 내용 저장ㄴ
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
