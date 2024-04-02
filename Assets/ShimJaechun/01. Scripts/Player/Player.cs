using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Jc
{
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private GameObject[] models;
        private int curModel = 0;

        [SerializeField]
        private Animator anim;
        public Animator Anim { get { return anim; }  }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        private float speed;
        public float Speed { get { return speed; } }

        [SerializeField]
        private float maxHp;
        public float MaxHp { get { return maxHp; } }

        [SerializeField]
        private float curHp;
        public float CurHp { get { return curHp; } }

        [SerializeField]
        private float atk;  // ���ݷ�
        public float ATK 
        {
            get 
            {
                return atk; 
            } 
            set 
            { 
                atk = value; 
            } 
        }

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        private PlayerFSM fsm;
        public PlayerFSM FSM { get { return fsm; } }

        [SerializeField]
        private PlayerTrigger trigger;
        public PlayerTrigger Trigger { get { return trigger; } }

        [SerializeField]
        private PlayerJoystickController controller;
        public PlayerJoystickController Controller { get { return controller; } }

        [SerializeField]
        private PlayerAttacker attacker;
        public PlayerAttacker Attacker { get { return attacker; } }

        [SerializeField]
        private PlayerDigger digger;
        public PlayerDigger Digger { get { return digger; } }

        [SerializeField]
        private PlayerBuilder builder;
        public PlayerBuilder Builder { get { return builder; } }    

        private Ground currentGround;

        private void Awake()
        {          
            fsm.CreateFSM(this);
            trigger.OnGround += OnGround;
        }
        private void Start()
        {
            SetModel(curModel);
        }
        private void Update()
        {
            // �÷��̾� �̵� 
            controller.Move(speed, anim);
        }
        public void OnGround(Ground ground)
        {
            currentGround = ground;
        }
        private void SetModel(int index)
        {
            // �� �����Լ�
            // ����ó�� : �ε��� ������ ��� ���
            if (index >= models.Length || index < 0) return;

            // ���� ���� ��Ȱ��ȭ
            models[curModel]?.SetActive(false);

            // �� ���� �� Ȱ��ȭ
            curModel = index;
            models[curModel].SetActive(true);
            // �ִϸ����� ����
            anim = models[curModel].GetComponent<Animator>();
        }
        // �׽�Ʈ�� ĳ���� �� ����
        private void OnCharacterChange(InputValue value)
        {
            SetModel(value.Get<float>() < 0 ? curModel - 1 : curModel + 1);
        }
        public void OnClickInteractButton()
        {

        }

        public void Equip()
        {
        }
        public void UnEquip()
        { 
        }
    }
}
