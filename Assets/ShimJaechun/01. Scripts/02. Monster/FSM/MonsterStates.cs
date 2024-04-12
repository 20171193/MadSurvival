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

    // 풀링된 상태
    public class MonsterPooled : MonsterBaseState
    {

    }

    // 대기 상태
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
            // 일정시간 딜레이 이후 상태전환 -> 트래킹
            delayRoutine = owner.StartCoroutine(Extension.DelayRoutine(0.1f, () => owner.FSM.ChangeState("Tracking")));
        }
        public override void Exit()
        {
            // 예외처리 : 외부에서 상태가 전이된 경우 현 상태의 딜레이루틴 정지
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
            if (trackingRoutine != null)
                owner.StopCoroutine(trackingRoutine);

            owner.Anim.SetFloat("MoveSpeed", 0f);
            // 탈출 시 멈춤
            owner.Agent.isStopped = true;
        }

        IEnumerator TrackingRoutine()
        {
            while(true)
            {
                // 0.1초에 한번 씩 길찾기 갱신
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
            // 공격 중 멈춤
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
            // 회전
            owner.transform.forward = (currentTarget.transform.position - owner.transform.position).normalized;
            // 공격
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
