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
        // ���̺� �� ���� ���� ����Ʈ 
        public List<SpawnInfo> spawnList = new List<SpawnInfo>();
    }
}
