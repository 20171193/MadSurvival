using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Jc
{
    // 공격/중립공격형 동물의 상태
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

            // 타겟지점으로 트래킹 실행
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
            // 탈출 시 멈춤
            owner.Agent.isStopped = true;
        }

        private void GetPlayer(GameObject player)
        {
            // 추격 중 플레이어를 찾았다면 공격 상태로 전이
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

            // 목표물에 접근하지 못한 경우
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
            // 타깃 방향으로 몸체 회전
            owner.transform.forward = (currentTarget.transform.position - owner.transform.position).normalized;
            // 공격 실행
            owner.Anim.SetTrigger("OnAttack");
        }

        IEnumerator AttackRoutine()
        {
            Attack();
            yield return null;

            while(owner.AttackTrigger.CurrentTarget == currentTarget)
            {
                // 타깃이 벗어나지 않았다면 계속 공격
                yield return new WaitForSeconds(owner.Stat.ATS);
                Attack();
            }

            attackRoutine = null;
            owner.FSM.ChangeState("Tracking");
            yield return null;
        }
    }
}