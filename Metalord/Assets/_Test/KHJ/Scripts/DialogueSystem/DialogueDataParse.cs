using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDataParse : MonoBehaviour
{
    public TextAsset csvFile = default;

    public Dialogue[] ParseDialogue(TextAsset csvData)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        //ParseDialogue(csvData);
        //모든 값은 null값이 아니고 "" 값으로 들어감
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1;)
        {
            string[] row = data[i].Split(new char[] { ',' });
            Dialogue _dialogue = new Dialogue();
            //다이얼로그 클래스의 row[0] = ID, row[1] = 이름, row[2] = 대사, row[3] = 이동 대사 ID 번호
            _dialogue.dialogueID = row[0];
            _dialogue.speakerName = row[1];
            List<string> contextList = new List<string>();
            do
            {
                contextList.Add(row[2]);
                if (row[3] != "")
                {
                    //Debug.Log("여기 들어오나요 ?");
                    _dialogue.nextTextNum = row[3].ToString();
                    //Debug.LogFormat("{0} <=== 이게 row3", _dialogue.nextTextNum);
                }
                if (++i < data.Length - 1)
                {
                    row = data[i].Split(new char[] { ',' }); //data i번째의 string 들을 , 로 구분
                }
                else
                {
                    break;
                }
            }
            while (row[0].ToString() == ""); //|| row[3].ToString() != "");
            _dialogue.contextes = contextList.ToArray();
            dialogueList.Add(_dialogue);
        }
        //Debug.Log(dialogueList.Count);
        //각 헤더 설정해서 담기
        //for (int i = 0; i < dialogueList.Count; i++)
        //{
        //    Debug.LogFormat("{0} <=== 이게 row0 ID", dialogueList[i].dialogueID);
        //    Debug.LogFormat("{0} <=== 이게 row1 NAME", dialogueList[i].speakerName);
        //    Debug.LogFormat("{0} <=== 이게 row2 CONTEXT", dialogueList[i].contextes);
        //    Debug.LogFormat("{0} <=== 이게 row2 CONTEXT Length", dialogueList[i].contextes.Length);
        //    for(int j = 0; j < dialogueList[i].contextes.Length; j++)
        //    {
        //        //Debug.LogFormat("{0} contextes 대화 내용들 ", dialogueList[i].contextes[j]);
        //    }
        //    Debug.LogFormat("{0} <=== 이게 row3 NEXTNUM", dialogueList[i].nextTextNum);
        //}
        return dialogueList.ToArray();
    }
}
