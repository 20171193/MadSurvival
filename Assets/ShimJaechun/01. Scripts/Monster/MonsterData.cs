using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class MonsterData : ScriptableObject
    {
        [Header("이름")]
        public string monsterName;
        [Header("이동속도")]
        public float speed;
        [Header("공격력")]
        public float atk;
        [Header("공격속도")]
        public float ats;
        [Header("체력")]
        public float hp;
        [Header("방어력")]
        public float amr;
    }
}
