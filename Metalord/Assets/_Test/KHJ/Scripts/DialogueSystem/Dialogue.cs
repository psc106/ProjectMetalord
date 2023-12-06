using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string speakerName; //대사 치는 이름

    public string[] contextes; //대사 내용
}

[System.Serializable]
public class DialogueEvent
{
    public string speakerName;

    public Vector2 line;
    public Dialogue[] dialogues;
}
