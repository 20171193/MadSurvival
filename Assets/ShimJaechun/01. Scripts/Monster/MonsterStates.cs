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

    // ��� ����
    public class MonsterIdle : MonsterBaseState
    {
        private Coroutine delayRoutine;
        public MonsterIdle(Monster owner)
        {
            this.owner = owner;
        }

        public override void Enter()
        {
            // �����ð� ������ ���� ������ȯ -> Ʈ��ŷ
            delayRoutine = owner.StartCoroutine(Extension.DelayRoutine(0.1f, () => owner.FSM.ChangeState("Tracking")));
        }
        public override void Exit()
        {
            // ����ó�� : �ܺο��� ���°� ���̵� ��� �� ������ �����̷�ƾ ����
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
