using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class MonsterBaseState : BaseState
    {
        protected Monster owner;
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
            trackingRoutine = owner.StartCoroutine(TrackingRoutine());
        }

        public override void Exit()
        {
            if (trackingRoutine != null)
                owner.StopCoroutine(trackingRoutine);

            // 탈출 시 멈춤
            owner.Agent.isStopped = true;
        }

        IEnumerator TrackingRoutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(0.1f);
                owner.Tracking(owner.PlayerGround);
            }
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
            owner.Anim.SetTrigger("OnAttack");
        }

        public override void Update()
        {
            
        }
    }

    public class MonsterDie : MonsterBaseState
    { 
        public MonsterDie(Monster owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            owner.Anim.SetTrigger("OnDie");
        }
    }
}
