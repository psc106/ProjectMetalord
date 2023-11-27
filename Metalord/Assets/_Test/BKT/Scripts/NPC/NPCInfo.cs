using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfo", menuName = "ScriptableObjects/NPCInfo", order = 1)]
public class NPCInfo : ScriptableObject
{
    public string id { get; private set; }

    [Header("NPC 행동명")]
    public string generalName;

    [Header("요구사항")]
    public int levelRequirement;
    public NPCInfo[] npcPrerequisites;

    [Header("단계")]
    public GameObject[] npcStepPrefabs;

    private void OnValidate()
    {
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
    }
}
