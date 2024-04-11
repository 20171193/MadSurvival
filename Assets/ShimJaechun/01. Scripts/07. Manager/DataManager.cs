using System;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using Jc;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

namespace Jc
{
    public enum DataType
    {
        MonsterData,
        ObstacleData,
        AnimalData,
        DaysWaveData,
        DaysAnimalData,
        DaysObstacleData,
        ItemData
    }

    public struct DaysAnimalInfo
    {
        public string animalName;
        public int spawnCount;
        public DaysAnimalInfo(string animalName, int spawnCount)
        {
            this.animalName = animalName;
            this.spawnCount = spawnCount;
        }
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
    private string daysObstacleDataName = "Data/DaysObstacleData";
    private string daysAnimalDataName = "Data/DaysAnimalData";
    private string itemPrefabName = "Item/Item_";

    // key : 이름 / value : 프리팹
    public Dictionary<string, Monster> monsterDic;
    public Dictionary<string, Animal> animalDic;

    // 로드해서 관리될 Dictionary
    public Dictionary<string, MonsterData> monsterDataDic;
    public Dictionary<string, Dictionary<int,ObstacleData>> obstacleDataDic;
    public Dictionary<int, WaveData> daysWaveDataDic;
    public Dictionary<int, DaysObstacleData> daysObstacleDataDic;
    public Dictionary<string, AnimalData> animalDataDic;
    public Dictionary<int, List<DaysAnimalInfo>> daysAnimalDataDic;

    private void OnEnable()
    {
        monsterDataDic = new Dictionary<string, MonsterData>();
        obstacleDataDic = new Dictionary<string, Dictionary<int, ObstacleData>>();
        daysWaveDataDic = new Dictionary<int, WaveData>();
        daysObstacleDataDic = new Dictionary<int, DaysObstacleData>();
        animalDataDic = new Dictionary<string, AnimalData>();
        daysAnimalDataDic = new Dictionary<int, List<DaysAnimalInfo>>();

        LoadData(DataType.MonsterData);
        // 몬스터 등록
        RegistMonster();
        LoadData(DataType.DaysWaveData);
        LoadData(DataType.AnimalData);
        // 동물 등록
        RegistAnimal();
        LoadData(DataType.DaysAnimalData);
    }

    private void RegistMonster()
    {
        monsterDic = new Dictionary<string, Monster>();

        Monster[] monsterPrefabs = Resources.LoadAll<Monster>("Monster");
        if(monsterPrefabs.Length < 1)
        {
            Debug.Log("몬스터 프리팹데이터가 없습니다.");
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
    private void RegistAnimal()
    {
        animalDic = new Dictionary<string, Animal>();
        Animal[] animalPrefabs = Resources.LoadAll<Animal>("Animal");
        if(animalPrefabs.Length < 1)
        {
            Debug.Log("동물 프리팹 데이터가 없습니다.");
            return;
        }
        foreach(Animal animal in animalPrefabs)
        {
            string name = animal.AnimalName;
            if (animalDic.ContainsKey(name)) continue;

            animalDic.Add(name, animal);
            Manager.Pool.CreatePool(animal, animal.Size, 15);
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
                    CSVToAnimalDataDic(CSVReader.Read(animalDataName));
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
            case DataType.DaysObstacleData:
                try
                {
                    CSVToDaysObstacleDataDic(CSVReader.Read(daysObstacleDataName));
                    return;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return;
                }
            case DataType.DaysAnimalData:
                try
                {
                    CSVToDaysAnimalDataDic(CSVReader.Read(daysAnimalDataName));
                    return;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
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
            // 6 dropMeatPercent
            MonsterData loadedData = ScriptableObject.CreateInstance<MonsterData>();
            loadedData.monsterName = (string)csvData[i]["monsterName"];
            loadedData.speed = (float)csvData[i]["speed"];
            loadedData.atk = (float)csvData[i]["atk"];
            loadedData.ats = (float)csvData[i]["ats"];
            loadedData.hp = (float)csvData[i]["hp"];
            loadedData.amr = (float)csvData[i]["amr"];
            loadedData.dropMeatPercent = (float)csvData[i]["dropMeatPercent"];
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
            loadedData.amr = (float)csvData[i]["amr"];

            for(int j =0; j<3; j++)
            {
                string itemString = (string)csvData[i][$"drop{j}"];
                // 아이템 정보가 없을 경우 continue
                if (itemString.Length < 1) continue;

                // 받아온 정보를 아이템이름_개수로 분할
                string[] itemInfo = itemString.Split('_');
                DropItem prefab = Resources.Load<DropItem>($"{itemPrefabName}{itemInfo[0]}");
                int count = int.Parse(itemInfo[1]);
                loadedData.dropItems.Add(new DropItemInfo(prefab,count));
            }

            // 해당 분류의 장애물이 사전에 존재하지 않는경우
            if (!obstacleDataDic.ContainsKey(loadedData.obstacleName))
            {
                obstacleDataDic.Add(loadedData.obstacleName, new Dictionary<int, ObstacleData>());
                // 해당 레벨에 대한 정보 또한 생성
                obstacleDataDic[loadedData.obstacleName].Add(loadedData.level, loadedData);
            }
            else
            {
                // 해당 레벨의 정보가 존재하지 않는 경우
                if (!obstacleDataDic[loadedData.obstacleName].ContainsKey(loadedData.level))
                {
                    obstacleDataDic[loadedData.obstacleName].Add(loadedData.level, loadedData);
                }
            }
        }
    }
    private void CSVToDaysWaveDataDic(List<Dictionary<string, object>> csvData)
    {
        for(int i =0; i<csvData.Count; i++)
        {
            int day = (int)csvData[i]["day"];

            WaveData lodedData = null;
            if (!daysWaveDataDic.ContainsKey(day))
            {
                lodedData = ScriptableObject.CreateInstance<WaveData>();
                daysWaveDataDic.Add(day, lodedData);
            }
            else
            {
                lodedData = daysWaveDataDic[day];
            }

            int waveNum = (int)csvData[i]["wave"] - 1;
            // 기존 웨이브에 대한 데이터가 없는경우
            for(int j =0; j<10; j++)     // 총 몬스터의 개수만큼 반복
            {
                string monsterName = Define.TryGetMonsterName((Define.MonsterName)j);
                // 해당 몬스터가 없을 경우
                int spawnCount = (int)csvData[i][monsterName];
                lodedData.spawnList[waveNum].monsterList.Add(spawnCount);
            }
        }
    }
    private void CSVToDaysObstacleDataDic(List<Dictionary<string, object>> csvData)
    {
        for (int i = 0; i < csvData.Count; i++)
        {
            if (!daysObstacleDataDic.ContainsKey((int)csvData[i]["day"]))
            {
                DaysObstacleData inst = ScriptableObject.CreateInstance<DaysObstacleData>();

                for(int j=0; j<3;j++)
                {
                    inst.trees[j] = (int)csvData[i][$"Tree_{j}"];
                    inst.stones[j] = (int)csvData[i][$"Stone_{j}"];
                }

                daysObstacleDataDic.Add((int)csvData[i]["day"], inst);
            }
        }

    }

    private void CSVToAnimalDataDic(List<Dictionary<string, object>> csvData)
    {
        foreach(Dictionary<string,object> dic in csvData)
        {
            string name = (string)dic["animalName"];
            if (animalDataDic.ContainsKey(name)) continue;

            AnimalData data = ScriptableObject.CreateInstance<AnimalData>();
            data.hp = (float)dic["hp"];
            data.speed = (float)dic["speed"];
            data.atk = (float)dic["atk"];
            data.ats = (float)dic["ats"];
            data.amr = (float)dic["amr"];
            data.detectRange = (float)dic["detectRange"];
            data.wonderRange = (float)dic["wonderRange"];
            data.atkRange = (float)dic["atkRange"];
            data.dropMeatCount = (int)dic["dropMeat"];
            data.dropNiceMeatCount = (int)dic["dropNiceMeat"];
            animalDataDic.Add(name, data);
        }
    }
    private void CSVToDaysAnimalDataDic(List<Dictionary<string, object>> csvData)
    {
        foreach (Dictionary<string, object> dic in csvData)
        {
            int day = (int)dic["day"];
            if (daysAnimalDataDic.ContainsKey(day)) continue;
            daysAnimalDataDic.Add(day, new List<DaysAnimalInfo>());

            for(int i =0; i<6; i++)
            {
                if (string.IsNullOrEmpty((string)dic[$"animal{i}"])) continue;
                string[] temp = dic[$"animal{i}"].ToString().Split('_');
                daysAnimalDataDic[day].Add(new DaysAnimalInfo(temp[0], int.Parse(temp[1])));
            }
        }
    }
}