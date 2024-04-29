using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    // ���̽� ����
    public class BaseState
    {
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }
    }
}