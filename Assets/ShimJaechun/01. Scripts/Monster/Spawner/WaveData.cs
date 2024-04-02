using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct SpawnInfo
    {
        public string monsterName;
        public int count;
        public SpawnInfo(string monsterName, int count)
        {
            this.monsterName = monsterName;
            this.count = count;
        }
    }

    [Serializable]
    public class WaveData : ScriptableObject
    {
        // ���̺� �� ���� ���� ����Ʈ 
        public List<SpawnInfo> spawnList = new List<SpawnInfo>();
    }
}
