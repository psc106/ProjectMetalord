using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataManager : MonoBehaviour
{
    // 싱글톤
    public static DataManager instance;

    public SavedGamePlayData savedGamePlayData; // 게임 플레이에 관한 데이터
    public SavedSettingsData savedSettingsData; // 게임 환경설정에 관한 데이터
    
    private string path; // 로컬 저장경로

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        savedGamePlayData = new SavedGamePlayData();
        savedSettingsData = new SavedSettingsData();
    }

    //1.저장 알림 -> 2. json 파일에 데이터 저장
    public void SaveGameData()
    {
        GameEventsManager.instance.dataEvents.SaveData(); //전체에 저장 이벤트 발생
        SaveDataToFile(); // 매니저에서 저장
    }

    // 1.json 파일 데이터 불러오기 -> 2.불러오기 알림
    public void LoadGameData()
    {
        LoadDataFromFile();
        GameEventsManager.instance.dataEvents.LoadData(); //전체에 저장 이벤트 발생
    }

    // 파일에 데이터 저장
    private void SaveDataToFile()
    {
        string saveFolderPath = Path.Combine(Application.dataPath, "JsonData"); // json 폴더까지 경로
        string jsonFilePath = Path.Combine(saveFolderPath, "GameData.json"); // json 폴더 + 파일 명
        string jsonData = JsonUtility.ToJson(savedGamePlayData);

        File.WriteAllText(jsonFilePath, jsonData); //파일로 저장
    }

    // 파일에서 데이터 불러오기
    private void LoadDataFromFile()
    {
        string saveFolderPath = Path.Combine(Application.dataPath, "JsonData"); // json 폴더까지 경로
        string jsonFilePath = Path.Combine(saveFolderPath, "GameData.json"); // json 폴더 + 파일 명
        string jsonData = File.ReadAllText(jsonFilePath);

        JsonUtility.FromJsonOverwrite(jsonData,savedGamePlayData); // json 파일 덮어쓰기
    }


    ///// <summary>
    ///// 데이터 저장 함수
    ///// 231208 배경택
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="saveData"></param>
    ///// <param name="_fileName"></param>
    //public void SaveData<T>(T saveData, string _fileName)
    //{
    //    string data = JsonUtility.ToJson(saveData);
    //    File.WriteAllText(path + _fileName, data);
    //}

    ///// <summary>
    ///// 데이터 불러오는 함수
    ///// 231208 배경택
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="_filename"></param>
    ///// <returns></returns>
    //public T LoadData<T>(string _filename)
    //{
    //    string data = File.ReadAllText(path + _filename);
    //    return JsonUtility.FromJson<T>(data);
    //}
}
