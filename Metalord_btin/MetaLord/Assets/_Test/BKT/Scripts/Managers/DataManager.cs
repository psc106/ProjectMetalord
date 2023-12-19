using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

public class JsonDataArray<TKey, TValue>
{
    public List<DataDictionary<TKey, TValue>> data;
}

public class DataManager : MonoBehaviour
{
    // 싱글톤
    public static DataManager instance;

    public static SavedGamePlayData savedGamePlayData; // 게임 플레이에 관한 데이터
    public static SavedSettingsData savedSettingsData; // 게임 환경설정에 관한 데이터
    
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

        path = Application.persistentDataPath + "/";
    }

    /// <summary>
    /// 데이터 저장 함수
    /// 231208 배경택
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="saveData"></param>
    /// <param name="_fileName"></param>
    public void SaveData<T>(T saveData, string _fileName)
    {
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path + _fileName, data);
    }

    /// <summary>
    /// 데이터 불러오는 함수
    /// 231208 배경택
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_filename"></param>
    /// <returns></returns>
    public T LoadData<T>(string _filename)
    {
        string data = File.ReadAllText(path + _filename);
        return JsonUtility.FromJson<T>(data);
    }
}
