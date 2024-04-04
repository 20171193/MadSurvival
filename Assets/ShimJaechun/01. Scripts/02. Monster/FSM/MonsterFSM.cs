using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{

    public class MonsterFSM : MonoBehaviour
    {
        private StateMachine<Monster> fsm;
        public StateMachine<Monster> FSM { get { return fsm; } }

        public string currentState;

        // FSM 생성
        public void CreateFSM(Monster owner)
        {
            fsm = new StateMachine<Monster>(owner);

            fsm.AddState("Pooled", new MonsterPooled());
            fsm.AddState("Idle",new MonsterIdle(owner));
            fsm.AddState("Tracking", new MonsterTracking(owner));
            fsm.AddState("Attack", new MonsterAttack(owner));
            fsm.AddState("Die", new MonsterDie(owner));

            // 초기 상태를 Idle상태로 지정
            fsm.Init("Pooled");
        }

        public void ChangeState(string state)
        {
            fsm.ChangeState(state);
        }

        private void Update()
        {
            currentState = fsm.CurState;
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
