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

        protected override void Awake()
        {
            base.Awake();

            fsm.CreateFSM(this);
            // ȸ���� ���� �����߰�
            fsm.FSM.AddState("Evade", new AnimalEvade(this));
            fsm.FSM.Init("Pooled");

            // �߸��� ������ ������ ������� ȸ��
            if (isNeutral)
                trigger.OnTakeDamage += DamagedEvade;
            // ȸ������ �������� ���� ��� ȸ��
            else
                detecter.OnDetectTarget += DetectedEvade;
        }
        protected override void OnEnable()
        {
            // ȸ���� ���ʹ� ������ ȸ��
            // �߸��� ���ʹ� ������ ������ ��� ȸ��
            // ��, Neutral�� �ƴ� ���� ������ ȸ�� 
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
