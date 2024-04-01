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
        MonsterData
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
    public Dictionary<string, MonsterData> monsterDataDic;

    private void OnEnable()
    {
        monsterDataDic = new Dictionary<string, MonsterData>();
        LoadMonsterData();
    }
    //public void SaveData(DataType type, int index = 0)
    //{
    //    string dataPath = "";
    //    switch(type)
    //    {
    //        case DataType.MosterData:
    //            dataPath = monsterDataPath;
    //            break;
    //        default:
    //            break;
    //    }

    //    if (Directory.Exists(dataPath) == false)
    //    {
    //        Directory.CreateDirectory(dataPath);
    //    }

    //    string json = JsonUtility.ToJson(dataPath, true);
    //    File.WriteAllText($"{dataPath}/{index}.txt", json);
    //}
    public void LoadMonsterData()
    {
        //if (File.Exists($"{dataPath}/{monsterDataName}.txt") == false)
        //{
        //    Debug.Log($"{dataPath}/{monsterDataName}.txt : 경로에 데이터가 존재하지 않습니다. ");
        //    return;
        //}

        //string csv = File.ReadAllText(($"{dataPath}/{monsterDataName}.txt").Replace("Resources",""));
        
        try
        {
            CsvToMonsterDataDic(CSVReader.Read(monsterDataName));
            Debug.Log("Load Success");
            return;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Load data fail : {ex.Message}");
            return;
        }
    }
    private void CsvToMonsterDataDic(List<Dictionary<string, object>> csvData)
    {
        for(int i =0; i < csvData.Count; i++)
        {
            // 0 monsterName
            // 1 speed
            // 2 atk
            // 3 ats
            // 4 hp
            // 5 amr
            MonsterData loadedData = new MonsterData();
            loadedData.monsterName = (string)csvData[i]["monsterName"];
            loadedData.speed = (float)csvData[i]["speed"];
            loadedData.atk = (float)csvData[i]["atk"];
            loadedData.ats = (float)csvData[i]["ats"];
            loadedData.hp = (float)csvData[i]["hp"];
            loadedData.amr = (float)csvData[i]["amr"];
            monsterDataDic.Add(loadedData.monsterName, loadedData);
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
}