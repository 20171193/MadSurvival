using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using jungmin;

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
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        private PlayerFSM fsm;
        public PlayerFSM FSM { get { return fsm; } }

        [SerializeField]
        private PlayerStat stat;
        public PlayerStat Stat { get { return stat; } }

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

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        public Ground currentGround;
        [SerializeField]
        private bool isAttackCoolTime = false;
        [SerializeField]
        private float curSpeed;

        private Coroutine damageRoutine;
        private Coroutine atsRoutine;

        private void Awake()
        {
            fsm.CreateFSM(this);
            trigger.owner = this;
        }
        private void Update()
        {
            Move();
        }
        private void Move()
        {
            // 플레이어 이동 
            controller.Move(Stat.MaxSpeed, ref curSpeed, anim);
            // 스테미너 처리
            if (curSpeed >= stat.speedThreshold)
            {
                if (stat.OwnStamina > 0)
                    stat.OwnStamina -= stat.staminaDecValue * Time.deltaTime;
            }
            else
            {
                if (stat.OwnStamina < stat.MaxStamina)
                    stat.OwnStamina += stat.staminaIncValue * Time.deltaTime;
            }
        }
        public void OnClickInteractButton()
        {
            if (isAttackCoolTime) return;
            
            anim.SetTrigger("OnAttack");

            if (atsRoutine != null)
                StopCoroutine(atsRoutine);
            // 공격 쿨타임 적용
            isAttackCoolTime = true;
            atsRoutine = StartCoroutine(AttackSpeedRoutine());
        }
        IEnumerator AttackSpeedRoutine()
        {
            yield return new WaitForSeconds(stat.ATS);
            isAttackCoolTime = false;
            atsRoutine = null;
        }

        #region 낮 / 밤 관련 이벤트처리
        public void OnEnterNight()
        {
            stat.StopTimer(CoreStatType.Hunger, false);
            stat.StopTimer(CoreStatType.Thirst, false);
        }
        public void OnEnterDay()
        {
            stat.StartTimer(false, CoreStatType.Hunger, 1f);
            stat.StartTimer(false, CoreStatType.Thirst, 1f);
        }
        #endregion

        #region 데미지 처리
        public void OnTakeDamage()
        {
            damageRoutine = StartCoroutine(DamageRoutine());
        }
        IEnumerator DamageRoutine()
        {
            // 일정시간 무적상태 적용

            float time = Stat.InvinsibleTime;
            float materialTime = Stat.InvinsibleTime / 10f;
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
                    materialTime = Stat.InvinsibleTime / 10f;
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
        #endregion

        #region 아이템 사용 / 장비
        public void GetItem(Item item)
        {
            backPack.AcquireItem(item);
        }
        public void Use(Item item)
        {

        }
        public void Equip()
        {

        }
        public void UnEquip()
        {
        }
        #endregion
    }
}
