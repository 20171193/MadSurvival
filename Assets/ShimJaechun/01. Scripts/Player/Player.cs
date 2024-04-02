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
        public Animator Anim { get { return anim; } }

        [SerializeField]
        private SkinnedMeshRenderer meshRenderer;
        [SerializeField]
        private Material invinsibleMT;
        [SerializeField]
        private Material originMT;

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
        public float CurHp { get { return curHp; } set { curHp = value; } }

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
        [SerializeField]
        private float ats;  // ���ݷ�
        public float ATS
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
        [SerializeField]
        private float amr;  // ����
        public float AMR
        {
            get
            {
                return amr;
            }
            set
            {
                amr = value;
            }
        }

        [SerializeField]
        private float invinsibleTime;   // �����ð� 

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

        public Ground currentGround;
        private Coroutine damageRoutine;
        private void Awake()
        {
            fsm.CreateFSM(this);
            trigger.owner = this;
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
            meshRenderer = models[curModel].transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        }
        // �׽�Ʈ�� ĳ���� �� ����
        private void OnCharacterChange(InputValue value)
        {
            SetModel(value.Get<float>() < 0 ? curModel - 1 : curModel + 1);
        }
        public void OnClickInteractButton()
        {

        }

        public void OnTakeDamage()
        {
            damageRoutine = StartCoroutine(DamageRoutine());
        }
        IEnumerator DamageRoutine()
        {
            // �����ð� �������� ����

            float time = invinsibleTime;
            float materialTime = invinsibleTime / 10f;
            bool isFadeOut = true;

            gameObject.layer = LayerMask.NameToLayer("Invinsible");
            meshRenderer.material = invinsibleMT;
            yield return null;

            while (time > 0f)
            {
                time -= Time.deltaTime;

                meshRenderer.material.color = new Color(
                meshRenderer.material.color.r,
                meshRenderer.material.color.g,
                meshRenderer.material.color.b,
                meshRenderer.material.color.a + (isFadeOut ? -Time.deltaTime : Time.deltaTime));

                materialTime -= Time.deltaTime;
                if (materialTime <= 0f)
                {
                    isFadeOut = !isFadeOut;
                    materialTime = invinsibleTime / 10f;
                }
                yield return null;
            }

            gameObject.layer = LayerMask.NameToLayer("Player");
            meshRenderer.material = originMT;
            yield return null;
        }
        public void Equip()
        {
        }
        public void UnEquip()
        {
        }
    }
}
