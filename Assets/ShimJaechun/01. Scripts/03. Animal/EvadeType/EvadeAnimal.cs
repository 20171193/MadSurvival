using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class EvadeAnimal : Animal
    {
        [Header("������ ���� - AI����")]
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
            // ȸ���� ���� �����߰�
            fsm.FSM.AddState("Evade", new AnimalEvade(this));
            fsm.FSM.Init("Pooled");

            detecter.OffDetectTarget += LoseTarget;
            detecter.OnDetectTarget += OnDetectTarget;
            trigger.OnTakeDamage += OnEvade;
        }
        protected override void OnEnable()
        {
            // ȸ���� ���ʹ� ������ ȸ��
            // �߸��� ���ʹ� ������ ������ ��� ȸ��
            // ��, Neutral�� �ƴ� ���� ������ ȸ�� 
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

            // ȸ���� ������ ��� Ž������ �� �ٷ� ȸ��
            if (!isNeutral)
                OnEvade();
        }
        public void LoseTarget()
        {
            curTarget = null;
        }
    }
}
