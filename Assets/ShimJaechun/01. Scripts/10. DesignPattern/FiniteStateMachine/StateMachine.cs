using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 트랜지션을 통한 상태전이 
    public struct Transition
    {
        public string end;
        public Func<bool> condition;
        public Transition(string end, Func<bool> condition)
        {
            this.end = end;
            this.condition = condition;
        }
    }

    public class StateMachine<TOwner>
    {
        // 상태머신 소유자
        private TOwner owner;
        public TOwner Owner { get { return owner; } }

        // 현재 상태
        private string curState;
        public string CurState { get { return curState; } }

        // 딕셔너리를 통한 상태 검색
        private Dictionary<string, BaseState> stateDic;
        public Dictionary<string, BaseState> StateDic { get { return stateDic; } }

        private List<Transition> anyStateTransition;

        public StateMachine(TOwner owner)
        {
            this.owner = owner;
            stateDic = new Dictionary<string, BaseState>();
            anyStateTransition = new List<Transition>();
        }

        public void AddState(string key, BaseState value)
        {
            stateDic.Add(key, value);
        }

        // AnyState 추가
        // 어떤 상태에서든 전이 가능
        public void AddAnyState(string key, Func<bool> condition)
        {
            anyStateTransition.Add(new Transition(key, condition));
        }

        // 트랜지션 추가
        public void AddTransition(string start, string end, Func<bool> condition)
        {
            stateDic[start].Transitions.Add(new Transition(end, condition));
        }

        // 초기 상태 지정
        public void Init(string entry)
        {
            curState = entry;
            stateDic[entry].Enter();
        }

        // 상태 전이
        public void ChangeState(string nextState)
        {
            // 현재 상태 탈출 -> 다음상태 진입
            // 이전 상태의 Exit이 먼저 호출되고 다음 상태의 Enter호출
            stateDic[curState].Exit();
            curState = nextState;
            stateDic[curState].Enter();
        }


        public void Update()
        {
            stateDic[curState].Update();

            // 우선순위 1
            // AnyState 확인
            foreach (var transition in anyStateTransition)
            {
                if (transition.condition() && transition.end != curState)
                {
                    ChangeState(transition.end);
                    return;
                }
            }

            // 상태 전이 확인
            foreach (var transition in stateDic[curState].Transitions)
            {
                if (transition.condition())
                {
                    ChangeState(transition.end);
                    return;
                }
            }
        }

        public void LateUpdate()
        {
            stateDic[curState].LateUpdate();
        }
        public void FixedUpdate()
        {
            stateDic[curState].FixedUpdate();
        }
    }
}