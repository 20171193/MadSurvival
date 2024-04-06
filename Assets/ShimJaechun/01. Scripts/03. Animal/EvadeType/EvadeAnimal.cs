using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class EvadeAnimal : Animal
    {
        [Header("에디터 세팅 - AI스텟")]
        [SerializeField]
        private float loseDelayTime;
        public float LoseDelayTime { get { return loseDelayTime; } }

        [SerializeField]
        private float evadeDistance;
        public float EvadeDistance { get { return evadeDistance; } }

        [SerializeField]
        private PlayerTrigger curTarget;
        public PlayerTrigger CurTarget { get { return curTarget; } }

        protected override void Awake()
        {
            base.Awake();

            fsm.CreateFSM(this);
            // 회피형 몬스터 상태추가
            fsm.FSM.AddState("Evade", new AnimalEvade(this));
            fsm.FSM.Init("Pooled");

            detecter.OffDetectTarget += LoseTarget;
            detecter.OnDetectTarget += OnDetectTarget;
            trigger.OnTakeDamage += OnEvade;
        }
        protected override void OnEnable()
        {
            // 회피형 몬스터는 무조건 회피
            // 중립형 몬스터는 공격을 당했을 경우 회피
            // 즉, Neutral이 아닌 경우는 무조건 회피 
            base.OnEnable();
        }

        public void OnEvade()
        {
            if (fsm.FSM.CurState != "Evade")
                fsm.ChangeState("Evade");
        }
        public void OnDetectTarget(PlayerTrigger player)
        {
            curTarget = player;

            // 회피형 동물의 경우 탐지했을 때 바로 회피
            if (!isNeutral)
                OnEvade();
        }
        public void LoseTarget()
        {
            curTarget = null;
        }
    }
}
