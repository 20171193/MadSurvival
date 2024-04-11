using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Jc
{
    public class Animal : PooledObject
    {
        [Header("������ ����")]
        [Space(2)]
        [SerializeField]
        protected string animalName;
        public string AnimalName { get { return animalName; } }

        [SerializeField]
        protected Rigidbody rigid;
        public Rigidbody Rigid { get { return rigid; } }

        [SerializeField]
        protected NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } }

        [SerializeField]
        protected Animator anim;
        public Animator Anim { get { return anim; } }

        [SerializeField]
        private SphereCollider detectCol;
        public SphereCollider DetectCol { get { return detectCol; } }

        // �߸��� ��������?
        [SerializeField]
        protected bool isNeutral;
        public bool IsNeutral { get { return isNeutral; } }

        [SerializeField]
        private ExplosionInvoker explosionInvoker;

        [SerializeField]
        private DropItem meat;
        [SerializeField]
        private DropItem niceMeat;

        [SerializeField]
        private int dropMeatCount;
        public int DropMeatCount { get { return dropMeatCount; } set { dropMeatCount = value; } }

        [SerializeField]
        private int dropNiceMeatCount;
        public int DropNiceMeatCount { get { return dropNiceMeatCount; } set { dropNiceMeatCount = value; } }

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        protected AnimalFSM fsm;
        public AnimalFSM FSM { get { return fsm; } }

        [SerializeField]
        protected AnimalStat stat;
        public AnimalStat Stat { get { return stat;} }

        [SerializeField]
        protected AnimalDetecter detecter;
        public AnimalDetecter Detecter { get { return detecter; } }

        [SerializeField]
        protected AnimalTrigger trigger;
        public AnimalTrigger Trigger { get { return trigger; } }

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]

        [SerializeField]
        public Ground onGround;

        [SerializeField]
        public Ground playerGround;

        [SerializeField]
        protected PlayerTrigger curTarget;
        public PlayerTrigger CurTarget { get { return curTarget; } }

        protected virtual void Awake()
        {
            detecter.OnDetectTarget += OnDetectTarget;
            detecter.OffDetectTarget += OnLoseTarget;
        }
        protected virtual void Start()
        {
            stat.InitSetting();
            detectCol.radius = stat.DetectRange;
        }
        public virtual void OnDetectTarget(PlayerTrigger player)
        {
            curTarget = player;
        }
        public virtual void OnLoseTarget()
        {
            curTarget = null;
        }

        public void DropItem()
        {
            // spawnCount��ŭ ����
            for (int i = 0; i < dropMeatCount; i++)
            {
                // �������� 1�� �� ������ ������ ���� ����
                Vector2 rand = UnityEngine.Random.insideUnitCircle;
                // �ش� �������� ������ġ ����
                Vector3 spawnPos = new Vector3(transform.position.x + rand.x, transform.position.y + 0.1f, transform.position.z + rand.y);

                Manager.Pool.GetPool(meat, spawnPos, Quaternion.identity);
            }
            // spawnCount��ŭ ����
            for (int i = 0; i < dropNiceMeatCount; i++)
            {
                // �������� 1�� �� ������ ������ ���� ����
                Vector2 rand = UnityEngine.Random.insideUnitCircle;
                // �ش� �������� ������ġ ����
                Vector3 spawnPos = new Vector3(transform.position.x + rand.x, transform.position.y + 0.1f, transform.position.z + rand.y);

                Manager.Pool.GetPool(niceMeat, spawnPos, Quaternion.identity);
            }

            // ������ �������� �߽����� ExplosionForce�� ����
            ExplosionInvoker invoker = (ExplosionInvoker)Manager.Pool.GetPool(explosionInvoker, transform.position, Quaternion.identity);
            invoker.OnExplosion();
        }

        // Ž������ �����
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stat.DetectRange);
        }
    }
}
