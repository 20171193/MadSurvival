using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class StateMachine<TOwner>
    {
        // ���¸ӽ� ������
        private TOwner owner;
        public TOwner Owner { get { return owner; } }

        // ���� ����
        private string curState;
        public string CurState { get { return curState; } }

        // ��ųʸ��� ���� ���� �˻�
        private Dictionary<string, BaseState> stateDic;
        public Dictionary<string, BaseState> StateDic { get { return stateDic; } }

        public StateMachine(TOwner owner)
        {
            this.owner = owner;
            stateDic = new Dictionary<string, BaseState>(); 
        }

        public void AddState(string key, BaseState value)
        {
            stateDic.Add(key, value);
        }

        // �ʱ� ���� ����
        public void Init(string entry)
        {
            curState = entry;
            stateDic[entry].Enter();
        }

        // ���� ����
        public void ChangeState(string nextState)
        {
            // ���� ���� Ż�� -> �������� ����
            // ���� ������ Exit�� ���� ȣ��ǰ� ���� ������ Enterȣ��
            stateDic[curState].Exit();
            curState = nextState;
            stateDic[curState].Enter();
        }


        public void Update()
        {
            stateDic[curState].Update();
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