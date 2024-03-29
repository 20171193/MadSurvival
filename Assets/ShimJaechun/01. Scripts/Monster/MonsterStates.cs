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
        public MonsterTracking(Monster owner)
        {
            this.owner = owner;
        }
    }

    public class MonsterAttack : MonsterBaseState
    {
        public MonsterAttack(Monster owner)
        {
            this.owner = owner;
        }
    }

    public class MonsterDie : MonsterBaseState
    {
        public MonsterDie(Monster owner)
        {
            this.owner = owner;
        }
    }
}
