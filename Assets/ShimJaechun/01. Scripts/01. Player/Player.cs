using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using jungmin;


namespace Jc
{
    public enum InteractButtonMode
    {
        None,
        Use,
        Attack,
        Build
    }
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [Space(2)]
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

        [SerializeField]
        private GameObject buildSocket;
        [SerializeField]
        private MeshRenderer buildSocketRenderer;

        [SerializeField]
        private Material enableSocketMT;
        [SerializeField]
        private Material disableSocketMT;

        [SerializeField]
        private GameObject joystick;
        [SerializeField]
        private GameObject interactButton;
        [SerializeField]
        private GameObject backpackButton;

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
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
        private PlayerItemController itemController;
        public PlayerItemController ItemController { get { return itemController; } }

        [SerializeField]
        private PlayerAttacker attacker;
        public PlayerAttacker Attacker { get { return attacker; } }

        [SerializeField]
        private PlayerBuilder builder;
        public PlayerBuilder Builder { get { return builder; } }

        [SerializeField]
        private ScoreboardInvoker score;
        public ScoreboardInvoker Score { get { return score; } }

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        public InteractButtonMode curButtonMode = InteractButtonMode.Attack;
        
        public Ground currentGround;

        [SerializeField]
        private bool isAttackCoolTime = false;

        [SerializeField]
        private float curSpeed;
        public float CurSpeed { get { return curSpeed; } }

        [SerializeField]
        private bool isOnBackpack = false;
        public bool IsOnBackpack { get { return isOnBackpack; } }

        [SerializeField]
        private bool isOnWater = false;
        public bool IsOnWater { get { return isOnWater; } set { isOnWater = value; } } 

        private Coroutine damageRoutine;
        private Coroutine atsRoutine;

        private void Awake()
        {
            trigger.owner = this;
        }
        private void Update()
        {
            Move();
        }
        private void Move()
        {
            // 플레이어 이동 
            controller.Move(Stat.CurMaxSpeed, ref curSpeed, anim);
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
        public void OnEnableMode(bool isEnable)
        {
            GetComponent<PlayerInput>().enabled = isEnable;
            joystick.SetActive(isEnable);
            interactButton.SetActive(isEnable);
            backpackButton.SetActive(isEnable);
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
            anim.SetBool("IsDie", true);
        }
        #endregion

        #region 아이템 사용 / 장비

        public void OnClickInteractButton()
        {
            switch(curButtonMode)
            {
                case InteractButtonMode.None:
                case InteractButtonMode.Attack:
                    {
                        if (isAttackCoolTime)
                            return;

                        anim.SetTrigger("OnAttack");

                        if (atsRoutine != null)
                            StopCoroutine(atsRoutine);
                        // 공격 쿨타임 적용
                        isAttackCoolTime = true;
                        atsRoutine = StartCoroutine(AttackSpeedRoutine());
                        // 무기를 장착하고 있는 경우
                        if (ItemController.CurWeaponItem != null)
                        {
                            ItemController.CurWeaponItem.durable--;
                            // 무기 내구도가 모두 소모된 경우
                            if(ItemController.CurWeaponItem.durable < 1)
                                ItemController.UnEquip(Equip_Item.EquipType.Weapon);
                        }
                        break;
                    }
                case InteractButtonMode.Use:
                    {
                        ItemController.Use();
                        break;
                    }
                case InteractButtonMode.Build:
                    {
                        Builder.Build((Build_Base)ItemController.CurQuickSlot.item);
                        break;
                    }
                default:
                    break;
            }
        }

        public void OpenBackPack()
        {
            backPack.TryOpenInventory();
            isOnBackpack = BackPackController.inventory_Activated;

            // 조이스틱, 상호작용 버튼 활성화/비활성화
            interactButton.SetActive(!isOnBackpack);
            joystick.SetActive(!isOnBackpack);

            if (!isOnBackpack)
                ItemController.ChangeButton();
        }
        public void GetItem(Item item)
        {
            backPack.AcquireItem(item);
        }
        // 무기 모델적용
        #endregion
    }
}
