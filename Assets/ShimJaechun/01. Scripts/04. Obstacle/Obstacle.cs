using Jc;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
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
        public string ObstacleName { get { return obstacleName; } set { obstacleName = value; } }

        [SerializeField]
        protected ObstacleType obstacleType;
        public ObstacleType ObstacleType { get { return obstacleType; } }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        protected int level;
        public int Level { get { return level; } set { level = value; } }

        [SerializeField]
        protected float hp;
        public float HP { get { return hp; } }

        [SerializeField]
        protected float ownHp;
        public float OwnHp { get { return ownHp; } }

        [SerializeField]
        protected float amr;
        public float AMR { get { return amr; } }

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private Ground onGround;

        [SerializeField]
        private ObstacleSpawner spawner;

        [SerializeField]
        private ObstacleData obstacleData;

        protected virtual void Awake()
        {
            spawnCount = spawnCount > size ? size : spawnCount;
        }

        public override void Release()
        {
            spawner.OnEnterNextDay -= Release;
            base.Release();
        }

        public void InitSetting(string name, int level, ObstacleSpawner spanwer)
        {
            if (!Manager.Data.obstacleDataDic.ContainsKey(name) ||
                !Manager.Data.obstacleDataDic[name].ContainsKey(level))
            {
                Debug.Log($"{name}({level}) : 의 데이터가 없습니다.");
                return;
            }

            this.spawner = spanwer;
            spawner.OnEnterNextDay += Release;
            obstacleData = Manager.Data.obstacleDataDic[name][level];
            // 모델 세팅
            levelSpecificModel[level].SetActive(true);
            StatSetting();
        }

        private void StatSetting()
        {
            // 데이터에 존재하는 스텟 적용
            obstacleName = obstacleData.obstacleName;
            level = obstacleData.level;
            hp = obstacleData.hp;
            ownHp = hp;
            amr = obstacleData.amr;
        }

        public void OnTile(Ground ground)
        {
            onGround = ground;
        }

        public virtual void Digged()
        {
            // 파괴 처리
            DropItem();
            levelSpecificModel[level].SetActive(false);
            Release();
        }
        protected virtual void DropItem()
        {
            if(obstacleData.dropItems.Count < 1)
            {
                Debug.Log($"{obstacleType} : 레벨{level} 아이템 리스트가 존재하지 않습니다.");
                return;
            }

            // 드랍할 아이템 리스트 할당
            foreach(DropItemInfo itemInfo in obstacleData.dropItems)
            {
                // 아이템이 존재하지 않을 경우 continue
                if (itemInfo.spawnCount < 1) continue;

                // 아이템 prefab을 
                DropItem prefab = itemInfo.prefab;

                // spawnCount만큼 생성
                for (int i = 0; i < itemInfo.spawnCount; i++)
                {
                    // 반지름이 1인 원 내부의 임의의 점을 도출
                    Vector2 rand = UnityEngine.Random.insideUnitCircle;
                    // 해당 아이템의 스폰위치 지정
                    Vector3 spawnPos = new Vector3(transform.position.x + rand.x, transform.position.y + 0.1f, transform.position.z + rand.y);

                    DropItem getItem = (DropItem)Manager.Pool.GetPool(prefab, spawnPos, Quaternion.identity);
                }
            }
            // 스폰할 기준점을 중심으로 ExplosionForce를 적용
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