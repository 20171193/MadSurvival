using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct DropItemInfo
    {
        // 아이템 프리팹
        public DropItem prefab;
        public int spawnCount;

        public DropItemInfo(DropItem prefab, int spawnCount)
        {
            this.prefab = prefab;
            this.spawnCount = spawnCount;
        }
    }
    public class ObstacleData : ScriptableObject
    {
        [Header("이름")]
        public string obstacleName;
        [Header("레벨")]
        public int level;
        [Header("체력")]
        public float hp;
        [Header("방어력")]
        public float amr;
        [Header("드롭 아이템 리스트")]
        public List<DropItemInfo> dropItems = new List<DropItemInfo>();
    }
}
