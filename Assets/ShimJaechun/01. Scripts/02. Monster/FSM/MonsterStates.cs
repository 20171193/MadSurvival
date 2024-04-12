using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class MonsterBaseState : BaseState
    {
        protected Monster owner;
    }

    // Ǯ���� ����
    public class MonsterPooled : MonsterBaseState
    {

    }

    // ��� ����
    public class MonsterIdle : MonsterBaseState
    {
        private Coroutine delayRoutine;
        public MonsterIdle(Monster owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            owner.GetComponent<CapsuleCollider>().enabled = true;
            owner.Agent.enabled = true;
            // �����ð� ������ ���� ������ȯ -> Ʈ��ŷ
            delayRoutine = owner.StartCoroutine(Extension.DelayRoutine(0.1f, () => owner.FSM.ChangeState("Tracking")));
        }
        public override void Exit()
        {
            // ����ó�� : �ܺο��� ���°� ���̵� ��� �� ������ �����̷�ƾ ����
            if (delayRoutine != null)
                owner.StopCoroutine(delayRoutine);
        }
    }

    public class MonsterTracking : MonsterBaseState
    {
        private Coroutine trackingRoutine;
        public MonsterTracking(Monster owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            // Ÿ���������� Ʈ��ŷ ����
            owner.Agent.isStopped = false;
            trackingRoutine = owner.StartCoroutine(TrackingRoutine());
        }

        public override void Update()
        {
            owner.Anim.SetFloat("MoveSpeed", owner.Agent.velocity.sqrMagnitude);
        }

        public override void Exit()
        {
            if (trackingRoutine != null)
                owner.StopCoroutine(trackingRoutine);

            owner.Anim.SetFloat("MoveSpeed", 0f);
            // Ż�� �� ����
            owner.Agent.isStopped = true;
        }

        IEnumerator TrackingRoutine()
        {
            while(true)
            {
                // 0.1�ʿ� �ѹ� �� ��ã�� ����
                yield return new WaitForSeconds(1.0f);
                owner.Detecter.Tracking(owner.Detecter.PlayerGround);
            }
        }
    }
    public class MonsterAttack : MonsterBaseState
    {
        private Coroutine attackRoutine;
        private GameObject currentTarget;
        public MonsterAttack(Monster owner)
        {
            this.owner = owner;
        }
        public override void Enter()
        {
            // ���� �� ����
            owner.Agent.isStopped = true;

            currentTarget = owner.Detecter.CurrentTarget;
            if(attackRoutine == null)
                attackRoutine = owner.StartCoroutine(AttackRoutine());
        }
        public override void Exit()
        {
            if (attackRoutine != null)
            {
                owner.StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
        }
        private void Attack()
        {
            // ȸ��
            owner.transform.forward = (currentTarget.transform.position - owner.transform.position).normalized;
            // ����
            owner.Anim.SetTrigger("OnAttack");
        }

        IEnumerator AttackRoutine()
        {
            Attack();
            yield return null;

            while (owner.Detecter.CurrentTarget == currentTarget)
            {
                Debug.Log($"owner : {owner.Detecter.CurrentTarget}, state : {currentTarget}");
                yield return new WaitForSeconds(owner.Stat.ATS);
                Attack();
            }

            attackRoutine = null;
            owner.FSM.ChangeState("Tracking");
            yield return null;
        }
    }
    public class MonsterDie : MonsterBaseState
    {
        private Coroutine dieRoutine;
        public MonsterDie(Monster owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            owner.DropItem();
            owner.Agent.isStopped = true;
            owner.Agent.enabled = false;
            owner.GetComponent<CapsuleCollider>().enabled = false;
            dieRoutine = owner.StartCoroutine(Extension.DelayRoutine(0.5f, ()=> owner.FSM.ChangeState("Pooled")));
        }

        public override void Exit()
        {
            owner.Release();
        }
    }
}
