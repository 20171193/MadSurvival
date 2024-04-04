using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct DropItemInfo
    {
        // ������ ������
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
        [Header("�̸�")]
        public string obstacleName;
        [Header("����")]
        public int level;
        [Header("ü��")]
        public float hp;
        [Header("����")]
        public float amr;
        [Header("��� ������ ����Ʈ")]
        public List<DropItemInfo> dropItems = new List<DropItemInfo>();
    }
}
