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

        protected override void Awake()
        {
            base.Awake();

            fsm.CreateFSM(this);
            // 회피형 몬스터 상태추가
            fsm.FSM.AddState("Evade", new AnimalEvade(this));
            fsm.FSM.Init("Pooled");

            // 중립형 동물은 공격을 받은경우 회피
            if (isNeutral)
                trigger.OnTakeDamage += DamagedEvade;
            // 회피형은 범위내에 들어온 경우 회피
            else
                detecter.OnDetectTarget += DetectedEvade;
        }
        protected override void OnEnable()
        {
            // 회피형 몬스터는 무조건 회피
            // 중립형 몬스터는 공격을 당했을 경우 회피
            // 즉, Neutral이 아닌 경우는 무조건 회피 
            base.OnEnable();
        }

        public void DamagedEvade()
        {
            Debug.Log("Damaged Evade");
            if (detecter.CurrentTarget != null &&
                fsm.FSM.CurState != "Evade")
                fsm.ChangeState("Evade");
        }
        private void DetectedEvade(GameObject obj)
        {
            Debug.Log("Detected Evade");
            if (detecter.CurrentTarget != null &&
                fsm.FSM.CurState != "Evade")
                fsm.ChangeState("Evade");
        }
    }
}
