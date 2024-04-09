using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class AnimalData : ScriptableObject
    {
        public float hp;            // 체력
        public float speed;         // 이동속도
        public float atk;           // 공격력
        public float ats;           // 공격속도
        public float amr;           // 방어력
        public float detectRange;   // 탐지범위
        public float wonderRange;   // 배회범위
        public float atkRange;      // 공격범위
    }
}
