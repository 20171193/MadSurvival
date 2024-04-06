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
            if(owner.CurTarget == null)


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
                yield return new WaitForSeconds(0.1f);
            }

            trackingRoutine = null;
            yield return null;
        }
    }

    public class AnimalAttack : AnimalBaseState
    {
        private AttackAnimal owner;
        private Coroutine attackRoutine;
        public AnimalAttack(Animal owner)
        {
            this.baseOwner = owner;
            this.owner = (AttackAnimal)owner;
        }

        public override void Enter()
        {
            owner.Anim.SetTrigger("OnAttack");
            attackRoutine = owner.StartCoroutine(Extension.DelayRoutine(0.5f, () => owner.FSM.ChangeState("Tracking")));
        }

        public override void Exit()
        {
            if (attackRoutine != null)
                owner.StopCoroutine(attackRoutine);
        }
    }
}