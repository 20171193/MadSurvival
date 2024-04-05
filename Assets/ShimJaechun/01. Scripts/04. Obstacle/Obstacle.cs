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
                Debug.Log($"{name}({level}) : �� �����Ͱ� �����ϴ�.");
                return;
            }

            this.spawner = spanwer;
            spawner.OnEnterNextDay += Release;
            obstacleData = Manager.Data.obstacleDataDic[name][level];
            // �� ����
            levelSpecificModel[level].SetActive(true);
            StatSetting();
        }

        private void StatSetting()
        {
            // �����Ϳ� �����ϴ� ���� ����
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
            // �ı� ó��
            DropItem();
            levelSpecificModel[level].SetActive(false);
            Release();
        }
        protected virtual void DropItem()
        {
            if(obstacleData.dropItems.Count < 1)
            {
                Debug.Log($"{obstacleType} : ����{level} ������ ����Ʈ�� �������� �ʽ��ϴ�.");
                return;
            }

            // ����� ������ ����Ʈ �Ҵ�
            foreach(DropItemInfo itemInfo in obstacleData.dropItems)
            {
                // �������� �������� ���� ��� continue
                if (itemInfo.spawnCount < 1) continue;

                // ������ prefab�� 
                DropItem prefab = itemInfo.prefab;

                // spawnCount��ŭ ����
                for (int i = 0; i < itemInfo.spawnCount; i++)
                {
                    // �������� 1�� �� ������ ������ ���� ����
                    Vector2 rand = UnityEngine.Random.insideUnitCircle;
                    // �ش� �������� ������ġ ����
                    Vector3 spawnPos = new Vector3(transform.position.x + rand.x, transform.position.y + 0.1f, transform.position.z + rand.y);

                    DropItem getItem = (DropItem)Manager.Pool.GetPool(prefab, spawnPos, Quaternion.identity);
                }
            }
            // ������ �������� �߽����� ExplosionForce�� ����
            ExplosionInvoker invoker = (ExplosionInvoker)Manager.Pool.GetPool(explosionInvoker, transform.position, Quaternion.identity);
            invoker.OnExplosion();
        }
        public void DigUp(float value)
        {
            Debug.Log($"{gameObject.name} DigUp");
            // ������ ó��
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