using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

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

        protected override void Awake()
        {
            base.Awake();

            fsm.CreateFSM(this);
            // 회피형 몬스터 상태추가
            fsm.FSM.AddState("Evade", new AnimalEvade(this));
            fsm.FSM.Init("Pooled");

            trigger.OnTakeDamage += OnEvade;
        }

        protected override void Start()
        {
            base.Start();
        }
        public void OnEvade()
        {
            if (FSM.FSM.CurState == "Die" || FSM.FSM.CurState == "ReturnPool") return;
            if (fsm.FSM.CurState != "Evade")  
                fsm.ChangeState("Evade");
        }
        public override void OnDetectTarget(PlayerTrigger player)
        {
            if (fsm.FSM.CurState == "Die" || fsm.FSM.CurState == "ReturnPool") return;

            base.OnDetectTarget(player);
            // 회피형 동물의 경우 탐지했을 때 바로 회피
            if (!isNeutral && player.owner.CurSpeed > 3f)
            {
                OnEvade();
            }
        }
    }
}
