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
    public class Monster : PooledObject, ITileable, IDamageable
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

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        private MonsterFSM fsm;
        public MonsterFSM FSM { get { return fsm; } }

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

        [SerializeField]
        private float knockBackPower;
        public float KnockBackPower { get { return knockBackPower; } }

        [SerializeField]
        private float knockBackTime;
        public float KnockBackTime { get { return knockBackTime; } }

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
            detecter.OnTrigger += FindTarget;
            detecter.OffTrigger += LoseTarget;
        }

        private void OnEnable()
        {
            // �÷��̾��� Ÿ�� ��ġ�� ����� ��� �߻��� �׼�
            Navi.OnChangePlayerGround += OnChangeTarget;
            // ���Ӹ� �Ҵ�
            detecter.PlayerGround = Navi.OnPlayerGround;

            // ���º���
            fsm.ChangeState("Idle");
        }

        private void Update()
        {
            currentState = fsm.currentState;
        }

        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        // ���� ���� Ʈ������ �׼����� ȣ��
        // Ʈ��ŷ �� �ֺ� ��ü�� ����� ��� ���� 
        public void FindTarget(GameObject target)
        {
            // �÷��̾� Ž��
            if (fsm.FSM.CurState == "Tracking" &&
                (detecter.IsTrackingPlayer && target.tag == "Player" ||
                !detecter.IsTrackingPlayer && Manager.Layer.wallLM.Contain(target.layer)))
            {
                detecter.CurrentTarget = target;
                fsm.ChangeState("Attack");
            }
        }
        public void LoseTarget(GameObject target)
        {
            if (target == detecter.CurrentTarget)
                detecter.CurrentTarget = null;
        }

        #region ������ ó��
        // ������ ó��
        public void TakeDamage(float value, Vector3 suspectPos)
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
                knockBackTimer = StartCoroutine(KnockBackRoutine());
            }
        }
        IEnumerator KnockBackRoutine()
        {
            // �׺�޽� ��Ȱ��ȭ
            // ���� �̵����� ��ȯ
            agent.enabled = false;
            rigid.AddForce(transform.forward * -knockBackPower, ForceMode.Impulse);

            yield return new WaitForSeconds(KnockBackTime);

            // ����
            rigid.velocity = Vector3.zero;
            agent.enabled = true;

            // �˹� �� �����Ĥ���
            agent.isStopped = true;
            yield return new WaitForSeconds(0.2f);
            agent.isStopped = false;
        }
        #endregion

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
        }
    }
}