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
        AnimalData,
        DaysWaveData
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
    private string daysWaveDataName = "Data/DaysWaveData";

    // key : 몬스터 이름 / value : 몬스터 프리팹
    public Dictionary<string, Monster> monsterDic;

    // 로드해서 관리될 Dictionary
    public Dictionary<string, MonsterData> monsterDataDic;
    public Dictionary<string, ObstacleData> obstacleDataDic;
    public Dictionary<int, Dictionary<int, WaveData>> daysWaveDataDic;
    public Dictionary<string, AnimalData> animalDataDic;

    private void OnEnable()
    {

        monsterDataDic = new Dictionary<string, MonsterData>();
        obstacleDataDic = new Dictionary<string, ObstacleData>();
        daysWaveDataDic = new Dictionary<int, Dictionary<int, WaveData>>();
        animalDataDic = new Dictionary<string, AnimalData>();

        LoadData(DataType.MonsterData);
        LoadData(DataType.ObstacleData);
        // 몬스터 등록
        RegistMonster();
        LoadData(DataType.DaysWaveData);
    }

    private void RegistMonster()
    {
        monsterDic = new Dictionary<string, Monster>();

        Monster[] monsterPrefabs = Resources.LoadAll<Monster>("Monster");
        if(monsterPrefabs.Length < 1)
        {
            Debug.Log("몬스터 데이터가 없습니다.");
            return;
        }

        foreach (Monster monster in monsterPrefabs)
        {
            string name = monster.MonsterName;
            if (monsterDic.ContainsKey(name)) continue;

            monsterDic.Add(name, monster);
            // 몬스터 등록 시 풀링
            Manager.Pool.CreatePool(monster, monster.Size, 20);
        }
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
            case DataType.DaysWaveData:
                try
                {
                    CSVToDaysWaveDataDic(CSVReader.Read(daysWaveDataName));
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

    private void CSVToDaysWaveDataDic(List<Dictionary<string, object>> csvData)
    {
        for(int i =0; i<csvData.Count; i++)
        {
            int day = (int)csvData[i]["day"];

            if (!daysWaveDataDic.ContainsKey(day))
                daysWaveDataDic.Add(day, new Dictionary<int, WaveData>());

            int waveNum = (int)csvData[i]["wave"];
            if (!daysWaveDataDic[day].ContainsKey(waveNum))
            {
                WaveData lodedData = ScriptableObject.CreateInstance<WaveData>();
                lodedData.spawnList.Add(new SpawnInfo((string)csvData[i]["monsterName"], (int)csvData[i]["spawnCount"]));
                daysWaveDataDic[day].Add(waveNum, lodedData);
            }
            else
                daysWaveDataDic[day][waveNum].spawnList.Add(new SpawnInfo((string)csvData[i]["monsterName"], (int)csvData[i]["spawnCount"]));
        }
    }
}