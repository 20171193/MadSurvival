using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Jc
{
    // ����/�߸������� ������ ����
    public class AnimalTracking : AnimalBaseState
    {
        private AttackAnimal owner;
        private Coroutine trackingRoutine;
        public AnimalTracking(Animal owner)
        {
            this.baseOwner = owner;
            this.owner = (AttackAnimal)owner;
        }
        public override void Enter()
        {
            owner.AttackTrigger.OnAttackTrigger += GetPlayer;

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
            owner.AttackTrigger.OnAttackTrigger -= GetPlayer;

            if (trackingRoutine != null)
                owner.StopCoroutine(trackingRoutine);

            owner.Anim.SetFloat("MoveSpeed", 0f);
            // Ż�� �� ����
            owner.Agent.isStopped = true;
        }

        private void GetPlayer(GameObject player)
        {
            // �߰� �� �÷��̾ ã�Ҵٸ� ���� ���·� ����
            Vector3 dir = (player.transform.position - owner.transform.position).normalized;
            owner.transform.forward = dir;
            owner.FSM.ChangeState("Attack");
        }

        IEnumerator TrackingRoutine()
        {
            float curTime = 0f;
            yield return null;

            while(curTime < owner.TrackingTime)
            {
                curTime += 0.1f;
                if (owner.IsLose == false)
                {
                    curTime = 0f;
                }
                owner.Tracker.Tracking(owner.playerGround);
                yield return new WaitForSeconds(0.1f);
            }

            // ��ǥ���� �������� ���� ���
            owner.FSM.ChangeState("Idle");
            trackingRoutine = null;
            yield return null;
        }
    }

    public class AnimalAttack : AnimalBaseState
    {
        private AttackAnimal owner;
        private Coroutine attackRoutine;
        private GameObject currentTarget;

        public AnimalAttack(Animal owner)
        {
            this.baseOwner = owner;
            this.owner = (AttackAnimal)owner;
        }

        public override void Enter()
        {
            owner.Agent.isStopped = true;
            currentTarget = owner.AttackTrigger.CurrentTarget;
            attackRoutine = owner.StartCoroutine(AttackRoutine());
        }

        public override void Exit()
        {
            owner.Agent.isStopped = false;

            if (attackRoutine != null)
                owner.StopCoroutine(attackRoutine);
        }

        private void Attack()
        {
            // Ÿ�� �������� ��ü ȸ��
            owner.transform.forward = (currentTarget.transform.position - owner.transform.position).normalized;
            // ���� ����
            owner.Anim.SetTrigger("OnAttack");
        }

        IEnumerator AttackRoutine()
        {
            Attack();
            yield return null;

            while(owner.AttackTrigger.CurrentTarget == currentTarget)
            {
                // Ÿ���� ����� �ʾҴٸ� ��� ����
                yield return new WaitForSeconds(owner.Stat.ATS);
                Attack();
            }

            attackRoutine = null;
            owner.FSM.ChangeState("Tracking");
            yield return null;
        }
    }
}