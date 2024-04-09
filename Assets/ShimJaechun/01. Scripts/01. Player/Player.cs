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
        [SerializeField]
        private bool isOnBackpack = false;
        public bool IsOnBackpack { get { return isOnBackpack; } }

        [SerializeField]
        private bool isBuildMode = false;
        [SerializeField]
        private bool isBuildable = false;

        [Space(3)]
        [Header("플레이어 아이템")]
        [Space(2)]
        [Header("등록된 슬롯")]
        [SerializeField]
        private Slot curQuickSlot;
        [SerializeField]
        private Slot curInventorySlot;
        [SerializeField]
        private Equip_Item curWeaponItem;
        [SerializeField]
        private Equip_Item curArmorItem;

        [Header("버튼 적용 아이템 이미지")]
        [SerializeField]
        private GameObject weaponImage;
        [SerializeField]
        private GameObject potionImage;
        [SerializeField]
        private GameObject buildImage;
        private GameObject curImage;

        [Header("캐릭터 무기 모델")]
        [SerializeField]
        private GameObject monsterWeaponModel;
        [SerializeField]
        private GameObject treeWeaponModel;
        [SerializeField]
        private GameObject stoneWeaponModel;
        private GameObject curWeaponModel;

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
            // 공격
            // 퀵슬롯에 아이템이 없거나 공격무기를 들고있는 경우
            if (curQuickSlot == null || curQuickSlot.item == null ||
                curQuickSlot.item.itemdata.itemtype == ItemData.ItemType.Equipment)
            {
                if (isAttackCoolTime) 
                    return;

                anim.SetTrigger("OnAttack");

                if (atsRoutine != null)
                    StopCoroutine(atsRoutine);
                // 공격 쿨타임 적용
                isAttackCoolTime = true;
                atsRoutine = StartCoroutine(AttackSpeedRoutine());

                return;
            }
            // 빌드 아이템
            if( curQuickSlot.item.itemdata.itemtype == ItemData.ItemType.Structure)
            {
                Build();
                return;
            }

            // 사용 아이템
            if (curQuickSlot.item.itemdata.itemtype == ItemData.ItemType.Used)
            {
                Use();
                return;
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

        }
        #endregion

        #region 아이템 사용 / 장비
        public void OpenBackPack()
        {
            backPack.TryOpenInventory();
            isOnBackpack = BackPackController.inventory_Activated;

            interactButton.SetActive(!isOnBackpack);
            joystick.SetActive(!isOnBackpack);

            if (!isOnBackpack)
                ChangeButton();
        }
        public void GetItem(Item item)
        {
            backPack.AcquireItem(item);
        }
        public void OnSelectQuickSlot(Slot slot)
        {
            // 기존 무기해제
            UnEquip(Equip_Item.EquipType.Weapon);

            curQuickSlot = slot;

            ChangeButton();
        }
        public void OnSelectInventorySlot(Slot slot)
        {
            curInventorySlot = slot;
        }
        private void ChangeButton()
        {
            if (curQuickSlot == null) return;
            if (curQuickSlot.item == null) return;

            curImage?.SetActive(false);

            switch (curQuickSlot.item.itemdata.itemtype)
            {
                case ItemData.ItemType.Used:
                    potionImage.SetActive(true);
                    curImage = potionImage;
                    break;
                case ItemData.ItemType.Equipment:
                    weaponImage.SetActive(true);
                    curImage = weaponImage;
                    break;
                case ItemData.ItemType.Structure:
                    buildImage.SetActive(true);
                    curImage = buildImage;
                    break;
            }
        }
        public void Use()
        {
            Slot curSlot = IsOnBackpack ? curInventorySlot : curQuickSlot;
            Used_Item item = (Used_Item)curSlot.item;
            if (item == null || 
                curSlot.ItemCount < 1) return;

            curSlot.ItemCount--;
            item.Use(this);

            if((curSlot == curQuickSlot) && curSlot.item == null || curSlot.itemCount < 1)
            {
                curImage?.SetActive(false);
            }
        }
        public void Equip()
        {
            Slot curSlot = IsOnBackpack ? curInventorySlot : curQuickSlot;

            Equip_Item item = (Equip_Item)curSlot.item;
            if (item == null) return;

            // 기존 아이템 장착해제
            UnEquip(item.equipType);
            switch (item.equipType)
            {
                case Equip_Item.EquipType.Weapon:
                    curWeaponItem = item;
                    anim.SetBool("IsTwoHand", true);
                    SetEquipModel(curWeaponItem.atkType);
                    break;
                case Equip_Item.EquipType.Armor:
                    curArmorItem = item;
                    break;
            }
            // 새 아이템 장착
            item.Equip(this);
        }
        public void UnEquip(Equip_Item.EquipType type)
        {
            if (curWeaponItem == null) return;
            // 무기 모델 해제
            curWeaponModel?.SetActive(false);

            switch (type)
            {
                case Equip_Item.EquipType.Weapon:
                    curWeaponItem.UnEquip(this);
                    anim.SetBool("IsTwoHand", false);
                    break;
                case Equip_Item.EquipType.Armor:
                    curArmorItem.UnEquip(this);
                    break;
            }
            curWeaponItem = null;
        }
        public void Build()
        { 
        }
        private void CheckBuildable()
        {

        }

        // 무기 모델적용
        private void SetEquipModel(Equip_Item.ATKType atkType)
        {
            curWeaponModel?.SetActive(false);

            switch(atkType)
            {
                case Equip_Item.ATKType.Monster:
                    curWeaponModel = monsterWeaponModel;
                    break;
                case Equip_Item.ATKType.Tree:
                    curWeaponModel = treeWeaponModel;
                    break;
                case Equip_Item.ATKType.Stone:
                    curWeaponModel = stoneWeaponModel;
                    break;
            }
            curWeaponModel?.SetActive(true);
        }
        #endregion
    }
}
