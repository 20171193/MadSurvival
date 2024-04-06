using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // ȸ��/�߸�ȸ���� ���� ����
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
            if (owner.CurTarget == null)
                evadeDir = -owner.transform.forward;
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
            // ���� ����
            if(owner.CurTarget != null)
                evadeDir = (owner.transform.position - owner.CurTarget.transform.position).normalized;

            // rotation
            owner.transform.forward = evadeDir;
            owner.Agent.Move(owner.transform.forward * owner.Stat.Speed * Time.deltaTime);

            if(owner.CurTarget == null
                && evadeRoutine == null)
            {
                evadeRoutine = owner.StartCoroutine(EvadeRoutine());
            }
        }
        IEnumerator EvadeRoutine()
        {
            // ��ǥ���� ��ģ �ð�üũ
            float curTime = 0f;
            yield return null;

            while(curTime < owner.LoseDelayTime)
            {
                curTime += 0.1f;

                if(owner.CurTarget != null)
                {
                    // ���� ���� �缳��
                    evadeDir = (owner.transform.position - owner.CurTarget.transform.position).normalized;
                    // ��ǥ���� ã�� ���
                    evadeRoutine = null;
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