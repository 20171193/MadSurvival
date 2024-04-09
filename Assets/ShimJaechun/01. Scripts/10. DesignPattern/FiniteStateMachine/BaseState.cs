using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // 베이스 상태
    public class BaseState
    {
        protected List<Transition> transitions = new List<Transition>();
        public List<Transition> Transitions { get { return transitions; } }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }
    }
}