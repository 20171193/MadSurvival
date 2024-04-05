using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class AnimalStat : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [Space(2)]
        [SerializeField]
        private Animal owner;
        public Animal Onwer { get { return owner; } }

        [SerializeField]
        private float idleTime;
        public float IdleTime { get { return idleTime; } }

        [SerializeField]
        private float wonderRange;
        public float WonderRange { get { return wonderRange; } }

        [Space(3)]
        [Header("코어 스텟")]
        [Space(2)]
        [SerializeField]
        private float maxHp;
        public float MaxHp { get { return maxHp; } }
        [SerializeField]
        private float ownHp;
        public float OwnHp { get { return ownHp; } set { ownHp = value; } }

        [SerializeField]
        private float speed;
        public float Speed { 
            get { return speed; }
            set
            {
                speed = value;
                owner.Agent.speed = value;
            }
        }

        [SerializeField]
        private float atk;
        public float ATK { get { return atk; } }

        [SerializeField]
        private float ats;
        public float ATS { get { return ats; } }

        [SerializeField]
        private float amr;
        public float AMR { get { return amr; } }

        [SerializeField]
        private float detectRange;
        public float DetectRange { get { return detectRange; } }

        private void OnEnable()
        {
            InitSetting();
        }
        private void InitSetting()
        {
            // 데이터 로드
            ownHp = maxHp;
            Speed = speed;
        }
    }
}
