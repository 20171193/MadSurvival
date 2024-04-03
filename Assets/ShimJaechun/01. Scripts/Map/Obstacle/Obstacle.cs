using Jc;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Jc
{
    public class Obstacle : PooledObject, IDiggable
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private DropItem dropItem;

        [Space(3)]
        [Header("Editor Setting")]
        [Space(2)]
        [SerializeField]
        protected int spawnCount;
        public int SpawnCount { get { return spawnCount; } }

        [SerializeField]
        protected string obstacleName;
        public string ObstacleName { get { return obstacleName; } }

        [SerializeField]
        protected DigType digType;
        public DigType DigType { get { return digType; } }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        protected int level;
        public int Level { get { return level; } }

        [SerializeField]
        protected float hp;
        public float HP { get { return hp; } }

        [SerializeField]
        protected float ownHp;
        public float OwnHp { get { return ownHp; } }

        protected virtual void Awake()
        {
            InitSetting();
            spawnCount = spawnCount > size ? size : spawnCount;
            GameObject inst = Instantiate(prefab, transform);
        }
        public void InitSetting()
        {
            if (!Manager.Data.obstacleDataDic.ContainsKey(obstacleName))
            {
                Debug.Log($"{obstacleName} : 의 데이터가 없습니다.");
                return;
            }

            ObstacleData loadedData = Manager.Data.obstacleDataDic[obstacleName];
            level = loadedData.level;
            hp = loadedData.hp;
            ownHp = hp;
        }

        protected void DropItem()
        {
            dropItem.OnDropItem();
        }

        public void DigUp(float value)
        {
            //if()
        }

        public DigType GetDigType()
        {
            return digType;
        }
    }
}