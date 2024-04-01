using System;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using Jc;

namespace Jc
{
    public enum DataType
    {
        MonsterData,
        ObstacleData,
        AnimalData
    }
}

public class DataManager : Singleton<DataManager>
{
#if UNITY_EDITOR
    private string dataPath => Path.Combine(Application.dataPath, $"Resources/Data");
#else
    private string dataPath => Path.Combine(Application.persistentDataPath, $"Resources/Data");
#endif
    private string monsterDataName = "Data/MonsterData";
    private string obstacleDataName = "Data/ObstacleData";
    private string animalDataName = "Data/AnimalData";

    public Dictionary<string, MonsterData> monsterDataDic;
    public Dictionary<string, ObstacleData> obstacleDataDic;
    public Dictionary<string, AnimalData> animalDataDic;

    private void OnEnable()
    {
        monsterDataDic = new Dictionary<string, MonsterData>();
        obstacleDataDic = new Dictionary<string, ObstacleData>();
        animalDataDic = new Dictionary<string, AnimalData>();

        LoadData(DataType.MonsterData);
        LoadData(DataType.ObstacleData);
    }
    public void LoadData(DataType type)
    {
        switch (type)
        {
            case DataType.MonsterData:
                try
                {
                    CSVToMonsterDataDic(CSVReader.Read(monsterDataName));
                    return;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Load data fail : {ex.Message}");
                    return;
                }
            case DataType.ObstacleData:
                try
                {
                    CSVToObstacleDataDic(CSVReader.Read(obstacleDataName));
                    return;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Load data fail : {ex.Message}");
                    return;
                }
            case DataType.AnimalData:
                try
                {
                    CSVToMonsterDataDic(CSVReader.Read(animalDataName));
                    return;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Load data fail : {ex.Message}");
                    return;
                }
            default:
                return;
        }
    }
    public bool ExistData(DataType type)
    {
        string dataName = "";
        switch (type)
        {
            case DataType.MonsterData:
                dataName = monsterDataName;
                break;
            default:
                break;
        }

        return File.Exists($"{dataPath}/{dataName}.txt");
    }

    // 데이터 변환
    private void CSVToMonsterDataDic(List<Dictionary<string, object>> csvData)
    {
        for (int i = 0; i < csvData.Count; i++)
        {
            // 0 monsterName
            // 1 speed
            // 2 atk
            // 3 ats
            // 4 hp
            // 5 amr
            MonsterData loadedData = ScriptableObject.CreateInstance<MonsterData>();
            loadedData.monsterName = (string)csvData[i]["monsterName"];
            loadedData.speed = (float)csvData[i]["speed"];
            loadedData.atk = (float)csvData[i]["atk"];
            loadedData.ats = (float)csvData[i]["ats"];
            loadedData.hp = (float)csvData[i]["hp"];
            loadedData.amr = (float)csvData[i]["amr"];
            monsterDataDic.Add(loadedData.monsterName, loadedData);
        }
    }
    private void CSVToObstacleDataDic(List<Dictionary<string, object>> csvData)
    {
        for (int i = 0; i < csvData.Count; i++)
        {
            // 0 obstacleName
            // 1 level
            // 2 hp
            ObstacleData loadedData = ScriptableObject.CreateInstance<ObstacleData>();
            loadedData.obstacleName = (string)csvData[i]["obstacleName"];
            loadedData.level = (int)csvData[i]["level"];
            loadedData.hp = (float)csvData[i]["hp"];
            obstacleDataDic.Add(loadedData.obstacleName, loadedData);
        }
    }
}