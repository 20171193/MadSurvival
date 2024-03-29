using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{

    public class MonsterFSM : MonoBehaviour
    {
        private StateMachine<Monster> fsm;
        public StateMachine<Monster> FSM { get { return fsm; } }

        // FSM ����
        public void CreateFSM(Monster owner)
        {
            fsm = new StateMachine<Monster>(owner);

            fsm.AddState("Idle",new MonsterIdle(owner));
            fsm.AddState("Tracking", new MonsterTracking(owner));
            fsm.AddState("Attack", new MonsterAttack(owner));
            fsm.AddState("Die", new MonsterDie(owner));

            // �ʱ� ���¸� Idle���·� ����
            fsm.Init("Idle");
        }

        public void ChangeState(string state)
        {
            fsm.ChangeState(state);
        }

        private void Update()
        {
            fsm.Update();
        }
        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }
        private void LateUpdate()
        {
            fsm.LateUpdate();
        }
    }
}
