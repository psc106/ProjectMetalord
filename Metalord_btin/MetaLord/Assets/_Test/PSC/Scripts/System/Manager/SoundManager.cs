using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance { get; private set; }

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioGroup[] audioGroups;

    Dictionary<int, AudioClip> audioNodes;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        audioNodes = new Dictionary<int, AudioClip>();

    }
    private void Start()
    {

        for (int i = 0; i < audioGroups.Length; i++)
        {
            var nodes = audioGroups[i].GetNodes();
            var group = audioGroups[i].GetGroup();

            for (int j = 0; j < nodes.Length; j++)
            {
                audioNodes.Add((int)group + j, nodes[j].Clip);
            }
        }
    }

    public void PlaySound(GroupList group, int id)
    {
        int key = (int)group + id;
        audioSource.PlayOneShot(audioNodes[key]);
    }
}

public enum GroupList
{
    Player = 0,
    Gun=100,
    Npc=200,
    UI=300,
    Item=400
}
public enum PlayerSoundList
{
    DefaultWalk = 0,
    GlueWalk,
    Jump 
}

public enum GunSoundList
{
}

public enum NpcSoundList
{
}

public enum UISoundList
{
}
public enum ItemSoundList
{
}