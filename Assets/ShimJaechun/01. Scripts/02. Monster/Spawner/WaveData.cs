using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Jc
{

    [Serializable]
    public struct SpawnInfo
    {
        public List<int> monsterList;
        public SpawnInfo(List<int> monsterList)
        {
            this.monsterList = monsterList;
        }
    }

    [Serializable]
    public class WaveData : ScriptableObject
    {
        // 웨이브 별 몬스터 스폰 리스트 
        public List<SpawnInfo> spawnList = new List<SpawnInfo>(3)
        {
            new SpawnInfo(new List<int>()),
            new SpawnInfo(new List<int>()),
            new SpawnInfo(new List<int>())
        };
    }
}
