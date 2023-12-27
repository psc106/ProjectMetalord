using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


/// <summary>
/// 움직이는 오브젝트 데이터 저장 및 로드를 위한 클래스
/// 231227 배경택
/// </summary>
public class MoveObjectData : MonoBehaviour
{
    public Vector3 pos;
    public Vector3 rotation;

    private void OnEnable()
    {
        GameEventsManager.instance.dataEvents.onSaveData += SaveObject;
        GameEventsManager.instance.dataEvents.onLoadData += LoadObject;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dataEvents.onSaveData -= SaveObject;
        GameEventsManager.instance.dataEvents.onLoadData -= LoadObject;
    }

    // 오브젝트 저장
    public void SaveObject()
    {
        pos = transform.localPosition;
        rotation = transform.eulerAngles;

        string fileName = gameObject.name;
        string saveFolderPath = Path.Combine(Application.dataPath, "JsonData"); // json 폴더까지 경로
        string jsonFilePath = Path.Combine(saveFolderPath, fileName + ".json"); // json 폴더 + 파일 명
        string jsonData = JsonUtility.ToJson(GetComponent<MoveObjectData>());   // 저장할 Json Data        
        
        File.WriteAllText(jsonFilePath, jsonData); //파일로 저장
    }

    // 오브젝트 불러오기
    public void LoadObject()
    {
        string fileName = gameObject.name;
        string saveFolderPath = Path.Combine(Application.dataPath, "JsonData"); // json 폴더까지 경로
        string jsonFilePath = Path.Combine(saveFolderPath, fileName + ".json"); // json 폴더 + 파일 명
        string jsonData = File.ReadAllText(jsonFilePath);                       // 불러올 Json Data

        JsonUtility.FromJsonOverwrite(jsonData,GetComponent<MoveObjectData>()); // json 파일 덮어쓰기

        transform.localPosition = pos;
        transform.eulerAngles = rotation;

        Debug.Log($"{pos.x} + {pos.y} + {pos.z}");
        Debug.Log($"{rotation.x} + {rotation.y} + {rotation.z}");
        Debug.Log("값이 변경 됬나 ?");
    }
}
