using Jc;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class Obstacle : PooledObject, IDiggable, ITileable
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private GameObject[] levelSpecificModel;

        [SerializeField]
        private ExplosionInvoker explosionInvoker;

        [Space(3)]
        [Header("Editor Setting")]
        [Space(2)]
        [SerializeField]
        private float popItemPower;
        [Range(0, 10f)]
        [SerializeField]
        private float popItemRange;

        [SerializeField]
        protected int spawnCount;
        public int SpawnCount { get { return spawnCount; } }

        [SerializeField]
        protected string obstacleName;
        public string ObstacleName { get { return obstacleName; } }

        [SerializeField]
        protected ObstacleType obstacleType;
        public ObstacleType ObstacleType { get { return obstacleType; } }

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

        public UnityAction<Obstacle> OnDigged;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private Ground onGround;

        protected virtual void Awake()
        {
            spawnCount = spawnCount > size ? size : spawnCount;
        }
        protected virtual void OnEnable()
        {
            InitSetting();
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
            levelSpecificModel[level].SetActive(true);
        }

        public void OnTile(Ground ground)
        {
            onGround = ground;
        }

        protected virtual void Digged()
        {
            // 파괴 처리
            DropItem();
            OnDigged?.Invoke(this);
            levelSpecificModel[level].SetActive(false);
            Release();
        }
        protected virtual void DropItem()
        {
            ItemData data = Manager.Data.itemDataDic[obstacleType];
            if(level < 0 || level >= data.level_SpecificItemLists.Count)
            {
                Debug.Log($"{obstacleType} : 레벨{level} 아이템 리스트가 존재하지 않습니다.");
                return;
            }

            // 드랍할 아이템 리스트 할당
            List<DropItem> dropItems = data.level_SpecificItemLists[level].dropItems;
            foreach(DropItem item in dropItems)
            {
                Vector2 rand = UnityEngine.Random.insideUnitCircle;
                Vector3 spawnPos = new Vector3(transform.position.x + rand.x, transform.position.y + 0.1f, transform.position.z + rand.y);
                DropItem getItem = (DropItem)Manager.Pool.GetPool(item, spawnPos, Quaternion.identity);
            }
            ExplosionInvoker invoker = (ExplosionInvoker)Manager.Pool.GetPool(explosionInvoker, transform.position, Quaternion.identity);
            invoker.OnExplosion();
        }
        public void DigUp(float value)
        {
            Debug.Log($"{gameObject.name} DigUp");
            // 데미지 처리
            ownHp -= 10f;
            if (ownHp <= 0f)
            {
                Digged();
            }
        }

        public ObstacleType GetObstacleType()
        {
            return obstacleType;
        }
    }
}