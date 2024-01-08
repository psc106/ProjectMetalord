using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataManager : MonoBehaviour
{
    // 싱글톤
    public static DataManager instance;

    public SavedGamePlayData savedGamePlayData; // 게임 플레이에 관한 데이터
    //public SavedSettingsData savedSettingsData; // 게임 환경설정에 관한 데이터

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        savedGamePlayData = new SavedGamePlayData();
        //savedSettingsData = new SavedSettingsData();        
    }

    //1.저장 알림 -> 2. json 파일에 데이터 저장
    public void SaveGameData()
    {
        //MainSceneManager.instance.InputIdToObject(); // 맵 오브젝트에 id값 부여
        GameEventsManager.instance.dataEvents.SaveData(); //전체에 저장 이벤트 발생
        SaveDataToFile(); // 매니저에서 저장
    }

    // 1.json 파일 데이터 불러오기 -> 2.불러오기 알림
    public void LoadGameData()
    {
        //MainSceneManager.instance.InputIdToObject(); // 맵 오브젝트에 id값 부여
        if (LoadDataFromFile()) GameEventsManager.instance.dataEvents.LoadData(); //전체에 저장 이벤트 발생
        else /* pass */ return;
    }

    // 파일에 데이터 저장
    private void SaveDataToFile()
    {
        // 240102 빌드테스트 전 저장경로
        //string saveFolderPath = Path.Combine(Application.dataPath, "JsonData"); // json 폴더까지 경로
        //string jsonFilePath = Path.Combine(saveFolderPath, "GameData.json"); // json 폴더 + 파일 명

        // 240102 빌드시 이용이 가능한 저장경로
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "JsonData");
        string jsonData = JsonUtility.ToJson(savedGamePlayData,true);

        File.WriteAllText(jsonFilePath, jsonData); //파일로 저장
    }

    // 파일에서 데이터 불러오기
    private bool LoadDataFromFile()
    {
        // 240102 빌드테스트 전 저장경로
        //string saveFolderPath = Path.Combine(Application.dataPath, "JsonData"); // json 폴더까지 경로
        //string jsonFilePath = Path.Combine(saveFolderPath, "GameData.json"); // json 폴더 + 파일 명

        // 240102 빌드시 이용이 가능한 저장경로
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "JsonData");
        string jsonData = default;

        if (File.Exists(jsonFilePath)) // 저장경로에 파일이 있다면
        {
            jsonData = File.ReadAllText(jsonFilePath);            
            JsonUtility.FromJsonOverwrite(jsonData,savedGamePlayData); // json 파일 덮어쓰기
            return true;
        }
        else return false;

    }

}
