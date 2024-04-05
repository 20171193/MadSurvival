using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 회피/중립회피형 몬스터 상태
    public class AnimalEvade : AnimalBaseState
    {
        private EvadeAnimal owner;
        private Coroutine evadeRoutine;
        private Vector3 evadeDir; 

        public AnimalEvade(Animal owner)
        {
            this.baseOwner = owner;
            this.owner = (EvadeAnimal)owner;
        }

        public override void Enter()
        {
        }
        public override void Update()
        {
            Evade();
            owner.Anim.SetFloat("MoveSpeed", owner.Agent.velocity.sqrMagnitude);
        }
        public override void Exit()
        {
            if (evadeRoutine != null)
                owner.StopCoroutine(evadeRoutine);
        }
        private void Evade()
        {
            // 방향 설정
            if(owner.Detecter.CurrentTarget != null)
                evadeDir = (owner.transform.position - owner.Detecter.CurrentTarget.transform.position).normalized;

            // rotation
            owner.transform.forward = evadeDir;
            owner.Agent.Move(evadeDir * owner.Stat.Speed * Time.deltaTime);

            if(owner.Detecter.CurrentTarget == null
                && evadeRoutine == null)
            {
                evadeRoutine = owner.StartCoroutine(EvadeRoutine());
            }
        }
        IEnumerator EvadeRoutine()
        {
            // 목표물을 놓친 시간체크
            float curTime = 0f;
            yield return null;

            while(curTime < owner.LoseDelayTime)
            {
                curTime += 0.1f;

                if(owner.Detecter.CurrentTarget != null)
                {
                    // 목표물을 찾은 경우
                    evadeRoutine = null;

                    // 도망 방향 재설정
                    evadeDir = (owner.transform.position - owner.Detecter.CurrentTarget.transform.position).normalized;
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            evadeRoutine = null;
            owner.FSM.ChangeState("Idle");
            yield return null;
        }
    }
}
