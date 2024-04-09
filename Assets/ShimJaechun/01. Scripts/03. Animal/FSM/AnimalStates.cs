using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Jc
{
    // 동물이 가질 공통 상태 
    public class AnimalBaseState : BaseState
    {
        protected Animal baseOwner;
    }
    public class AnimalPooled : AnimalBaseState
    {
    }
    public class AnimalIdle : AnimalBaseState
    {
        private Coroutine idleRoutine;
        public AnimalIdle(Animal owner)
        {
            this.baseOwner = owner;
        }
        public override void Enter()
        {
            baseOwner.Agent.enabled = true;
            baseOwner.Agent.isStopped = true;
            baseOwner.Anim.SetFloat("MoveSpeed", 0f);
            idleRoutine = baseOwner.StartCoroutine(Extension.DelayRoutine(baseOwner.Stat.IdleTime, () => baseOwner.FSM.ChangeState("Wonder")));
        }
        public override void Exit()
        {
            baseOwner.Agent.isStopped = false;

            if (idleRoutine != null)
                baseOwner.StopCoroutine(idleRoutine);
        }
    }
    public class AnimalWonder : AnimalBaseState
    {
        private Vector3 dest = Vector3.zero;
        private Coroutine wonderRoutine;

        public AnimalWonder(Animal owner)
        {
            this.baseOwner = owner;
        }

        public override void Enter()
        {
            if(baseOwner.Agent.isStopped)
                baseOwner.Agent.isStopped = false;

            // 에이전트 속도 적용
            baseOwner.Agent.speed = baseOwner.Stat.Speed /2f;
            SetDestination();
        }
        public override void Exit()
        {
            if (wonderRoutine != null)
                baseOwner.StopCoroutine(wonderRoutine);
        }
        private bool IsArrived()
        {
            return baseOwner.Agent.remainingDistance < 0.9f;
        }
        private void SetDestination()
        {
            // 반지름이 1인 원 내부의 임의의 점을 도출 (배회 방향)
            Vector2 rand = UnityEngine.Random.insideUnitCircle * baseOwner.Stat.WonderRange;
            // 해당 방향 * 배회 거리 지점을 목적지로 설정
            dest = new Vector3(baseOwner.transform.position.x + rand.x,
                baseOwner.transform.position.y,
                baseOwner.transform.position.z + rand.y);

            // 목적지를 설정 -> 이동
            baseOwner.Agent.destination = dest;
            // 도착지 확인용 코루틴 실행
            wonderRoutine = baseOwner.StartCoroutine(WonderRoutine());
        }

        // 배회 루틴 (목적지를 체크)
        IEnumerator WonderRoutine()
        {
            while(!IsArrived())
            {
                baseOwner.Anim.SetFloat("MoveSpeed", baseOwner.Agent.velocity.sqrMagnitude);
                yield return new WaitForSeconds(0.1f);
            }

            wonderRoutine = null;
            baseOwner.FSM.ChangeState("Idle");
            yield return null;
        }
    }
    public class AnimalDie : AnimalBaseState
    {
        private Coroutine dieRoutine;
        public AnimalDie(Animal owner)
        {
            this.baseOwner = owner;
        }
        public override void Enter()
        {
            baseOwner.Anim.SetBool("IsDie", true);
            baseOwner.Agent.isStopped = true;
            dieRoutine = baseOwner.StartCoroutine(Extension.DelayRoutine(1.5f, () => baseOwner.FSM.ChangeState("ReturnPool")));
        }

        public override void Exit()
        {
            if (dieRoutine != null)
                baseOwner.StopCoroutine(dieRoutine);

            baseOwner.Anim.SetBool("IsDie", false);
        }
    }
    public class AnimalReturnPool : AnimalBaseState
    {
        public AnimalReturnPool(Animal owner)
        {
            baseOwner = owner;
        }
        public override void Enter()
        {
            // 초기 세팅으로 원복
            baseOwner.Agent.enabled = false;
            baseOwner.Stat.InitSetting();
            baseOwner.Release();
            baseOwner.FSM.ChangeState("Pooled");
        }
    }
}
