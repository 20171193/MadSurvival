using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
namespace Jc
{
    public class Monster : PooledObject, ITileable, IDamageable, IKnockbackable
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private Rigidbody rigid;

        [SerializeField]
        private NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } }

        [SerializeField]
        private Animator anim;
        public Animator Anim { get { return anim; } }

        [SerializeField]
        private ExplosionInvoker explosionInvoker;

        [SerializeField]
        private DropItem meat;

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        private MonsterFSM fsm;
        public MonsterFSM FSM { get { return fsm; } }

        [SerializeField]
        private MonsterStat stat;
        public MonsterStat Stat { get { return stat; } }

        [SerializeField]
        private MonsterDetecter detecter;
        public MonsterDetecter Detecter { get { return detecter; } }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [Header("�� �����ؾ��ϴ� �κ�. �̸����� Find")]
        [SerializeField]
        protected string monsterName;
        public string MonsterName { get { return monsterName; } }

        private Coroutine knockBackTimer;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        public UnityAction<Monster> OnMonsterDie;
        public string currentState;

        // ����ȭ
        public NavigationManager Navi => Manager.Navi;


        private void Awake()
        {
            fsm.CreateFSM(this);
        }

        private void OnEnable()
        {
            // �÷��̾��� Ÿ�� ��ġ�� ����� ��� �߻��� �׼�
            Navi.OnChangePlayerGround += OnChangeTarget;
            // ���Ӹ� �Ҵ�
            detecter.PlayerGround = Navi.OnPlayerGround;
        }

        private void Update()
        {
            currentState = fsm.currentState;
        }

        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        #region ������ ó��
        // ������ ó��
        public void TakeDamage(float value)
        {
            // �������� ó��
            float damage = value - stat.AMR;
            if (damage <= 0) return;

            // ���ó��
            if(stat.OwnHp < damage)
            {
                fsm.ChangeState("Die");
            }
            // ������ ó��, �˹�
            else
            {
                anim.SetTrigger("OnHit");
                stat.OwnHp -= damage;
            }
        }

        public void Knockback(float power, float time, Vector3 suspectPos)
        {
            knockBackTimer = StartCoroutine(KnockBackRoutine(power, time, suspectPos));
        }

        IEnumerator KnockBackRoutine(float power, float time, Vector3 suspectPos)
        {
            // �׺�޽� ��Ȱ��ȭ
            // ���� �̵����� ��ȯ

            Vector3 dir = transform.position - suspectPos;

            agent.enabled = false;
            rigid.AddForce(dir * power, ForceMode.Impulse);

            yield return new WaitForSeconds(time);

            // ����
            rigid.velocity = Vector3.zero;
            agent.enabled = true;

            // �˹� �� ������
            agent.isStopped = true;
            yield return new WaitForSeconds(0.2f);
            agent.isStopped = false;
        }
        #endregion

        public void DropItem()
        {
            // ������ Ȯ�� ����
            if (stat.DropMeatPercent == 0) return;

            float rand = Random.Range(1, 10) / 10f;
            if (rand > stat.DropMeatPercent) return;

            Manager.Pool.GetPool(meat, transform.position + Vector3.up, Quaternion.identity);

            ExplosionInvoker invoker = (ExplosionInvoker)Manager.Pool.GetPool(explosionInvoker, transform.position, Quaternion.identity);
            invoker.OnExplosion();
        }

        #region �������̽� �������̵�
        // ���Ͱ� Ÿ�Ͽ� ������ ��� ����
        public void OnTile(Ground ground)
        {
            detecter.OnGround = ground;
        }

        // �÷��̾� ��ġ�� ����� ��� ȣ��� �Լ�
        public void OnChangeTarget(Ground playerGround)
        {
            detecter.PlayerGround = playerGround;
        }
        #endregion

        public override void Release()
        {
            base.Release();
            OnMonsterDie?.Invoke(this);
            ScoreboardInvoker.Instance.killMonster?.Invoke(ScoreType.Monster);
        }
    }
}