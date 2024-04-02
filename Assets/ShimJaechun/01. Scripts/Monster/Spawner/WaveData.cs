using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct SpawnInfo
    {
        public Monster monster;
        public int count;
        public SpawnInfo(Monster monster, int count)
        {
            this.monster = monster;
            this.count = count;
        }
    }

    [Serializable]
    public class WaveData : ScriptableObject
    {
        // 웨이브 별 몬스터 스폰 리스트 
        public List<SpawnInfo> spawnList = new List<SpawnInfo>();
    }
}
