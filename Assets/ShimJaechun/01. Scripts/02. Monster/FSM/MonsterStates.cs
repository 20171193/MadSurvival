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
        public MonsterTracking(Monster owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            // Ÿ���������� Ʈ��ŷ ����
            owner.Agent.isStopped = false;
            owner.Agent.destination = owner.Detecter.PlayerGround.transform.position;
        }
        public override void LateUpdate()
        {
            owner.Anim.SetFloat("MoveSpeed", owner.Agent.velocity.sqrMagnitude);
        }
        public override void Exit()
        {
            owner.Anim.SetFloat("MoveSpeed", 0f);
            // Ż�� �� ����
            //owner.Agent.isStopped = true;
        }
    }
    public class MonsterAttack : MonsterBaseState
    {
        private Coroutine attackRoutine;

        public MonsterAttack(Monster owner)
        {
            this.owner = owner;
        }
        public override void Enter()
        {
            // ���� �� ����
            //owner.Agent.isStopped = true;
            owner.Agent.destination = owner.Detecter.CurrentTarget.transform.position;

            if (attackRoutine == null)
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
        public override void Update()
        {
            if(owner.Agent.remainingDistance < 0.5f)
                owner.Agent.isStopped = true;
            else   
                owner.Agent.isStopped = false;

        }

        private void Attack()
        {
            // ȸ��
            owner.transform.forward = (owner.Detecter.CurrentTarget.transform.position - owner.transform.position).normalized;
            // ����
            owner.Anim.SetTrigger("OnAttack");
        }

        IEnumerator AttackRoutine()
        {
            while (owner.Detecter.CurrentTarget != null && owner.Detecter.CurrentTarget.activeSelf)
            {
                Attack();
                yield return new WaitForSeconds(owner.Stat.ATS);
            }

            owner.FSM.ChangeState("Tracking");
            attackRoutine = null;
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
