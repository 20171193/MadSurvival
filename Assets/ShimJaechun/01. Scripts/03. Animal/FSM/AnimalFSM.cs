using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class AnimalFSM : MonoBehaviour
    {
        private StateMachine<Animal> fsm;
        public StateMachine<Animal> FSM { get { return fsm; } }

        // 디버깅용
        public string currentState;

        public void CreateFSM(Animal owner)
        {
            fsm = new StateMachine<Animal>(owner);

            // 공통적으로 모든 동물들이 갖는 상태를 지정
            // 풀링, 대기, 배회, 사망, 리턴 (비활성화)
            fsm.AddState("Pooled", new AnimalPooled());
            fsm.AddState("Idle", new AnimalIdle(owner));
            fsm.AddState("Wonder", new AnimalWonder(owner));
            fsm.AddState("Die", new AnimalDie(owner));
            fsm.AddState("ReturnPool", new AnimalReturnPool(owner));

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
