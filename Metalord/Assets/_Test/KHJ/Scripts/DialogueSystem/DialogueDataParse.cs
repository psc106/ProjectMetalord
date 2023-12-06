using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDataParse : MonoBehaviour
{
    [SerializeField][TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;

    //Get 프로퍼티 람다식으로 정의 
    //public string[] Dialogue => dialogue;
    public string[] Dialogue
    {
        get { return dialogue; }
    }

    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => responses;


    public TextAsset csvData = default;
    void Start()
    {
        //ParseDialogue("tutorial");
        ParseDialogue(csvData);
    }
    public Dialogue[] ParseDialogue(TextAsset _csvData)//(string csvFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue> (); //대화 리스트 생성 
        //TextAsset csvData = Resources.Load<TextAsset>(csvFileName);

        string[] data = _csvData.text.Split(new char[] { '\n' }); //엔터기준 쪼갬

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });  // 콤마 기준으로 쪼갬
            
            Dialogue _dialogue = new Dialogue ();  //새로운 대화 생성
            
            _dialogue.speakerName = row[1];

            List<string> contextList = new List<string> ();
            do
            {
                contextList.Add(row[2]);
                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' }); //data i번째의 string 들을 , 로 구분
                }
                else
                {
                    break;
                }
            }
            while (row[0].ToString() == "");
            _dialogue.contextes = contextList.ToArray();
            
            dialogueList.Add(_dialogue);
        }
        return dialogueList.ToArray ();
    }
}
