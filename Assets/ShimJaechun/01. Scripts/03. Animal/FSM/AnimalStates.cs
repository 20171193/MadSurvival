using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Jc
{
    // ������ ���� ���� ���� 
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

            // ������Ʈ �ӵ� ����
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
            // �������� 1�� �� ������ ������ ���� ���� (��ȸ ����)
            Vector2 rand = UnityEngine.Random.insideUnitCircle * baseOwner.Stat.WonderRange;
            // �ش� ���� * ��ȸ �Ÿ� ������ �������� ����
            dest = new Vector3(baseOwner.transform.position.x + rand.x,
                baseOwner.transform.position.y,
                baseOwner.transform.position.z + rand.y);

            // �������� ���� -> �̵�
            baseOwner.Agent.destination = dest;
            // ������ Ȯ�ο� �ڷ�ƾ ����
            wonderRoutine = baseOwner.StartCoroutine(WonderRoutine());
        }

        // ��ȸ ��ƾ (�������� üũ)
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
            // �ʱ� �������� ����
            baseOwner.Agent.enabled = false;
            baseOwner.Stat.InitSetting();
            baseOwner.Release();
            baseOwner.FSM.ChangeState("Pooled");
        }
    }
}
