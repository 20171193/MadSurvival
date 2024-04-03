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
        private BackPackController backPack;

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
        private float monsterAtk;  // 공격력
        public float MonsterATK
        {get{return monsterAtk;}set{monsterAtk = value;}}

        [SerializeField]        // 트리 대상 공격력
        private float treeAtk;
        public float TreeATK { get { return treeAtk; } set { treeAtk = value; } }

        [SerializeField]
        private float stoneAtk; // 바위 대상 공격력
        public float StoneATK { get { return stoneAtk;}set { stoneAtk = value; } }

        [SerializeField]
        private float ats;  // 공격속도
        public float ATS
        {
            get
            {
                return ats;
            }
            set
            {
                ats = value;
            }
        }

        [SerializeField]
        private float amr;  // 방어력
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
        private float invinsibleTime;   // 무적시간 

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
            CurHp = maxHp;
        }
        private void Update()
        {
            // 플레이어 이동 
            controller.Move(speed, anim);
        }
        
        public void OnTakeDamage()
        {
            damageRoutine = StartCoroutine(DamageRoutine());
        }
        IEnumerator DamageRoutine()
        {
            // 일정시간 무적상태 적용

            float time = invinsibleTime;
            float materialTime = invinsibleTime / 10f;
            bool isFadeOut = true;

            trigger.gameObject.layer = LayerMask.NameToLayer("Invinsible");
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

            trigger.gameObject.layer = LayerMask.NameToLayer("Player");
            meshRenderer.material = originMT;
            yield return null;
        }
        public void OnDie()
        {

        }
        public void GetItem(Item item)
        {
            backPack.AcquireItem(item);
        }

        public void Equip()
        {
        }
        public void UnEquip()
        {
        }
    }
}
