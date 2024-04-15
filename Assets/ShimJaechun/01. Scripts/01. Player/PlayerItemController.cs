using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Buffers;

namespace Jc
{
    public class PlayerItemController : MonoBehaviour
    {
        [SerializeField]
        private Player owner;

        [Space(3)]
        [Header("�÷��̾� ������")]
        [Space(2)]
        [Header("��ư ���� ������ �̹���")]
        [SerializeField]
        private GameObject weaponImage;
        [SerializeField]
        private GameObject potionImage;
        [SerializeField]
        private GameObject buildImage;
        private GameObject curImage;

        [Header("ĳ���� ���� ��")]
        [SerializeField]
        private GameObject[] swordModel;
        [SerializeField]
        private GameObject[] axeModel;
        [SerializeField]
        private GameObject[] pickAxeModel;

        private GameObject curWeaponModel;

        [Header("ĳ���� ������ ��")]
        [SerializeField]
        private GameObject meatModel;
        [SerializeField]
        private GameObject niceMeatModel;
        private GameObject curItemModel;
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private Equip_Item curWeaponItem;
        public Equip_Item CurWeaponItem { get { return curWeaponItem; } }
        [SerializeField]
        private Equip_Item curArmorItem;
        public Equip_Item CurArmorItem { get { return curArmorItem; } }
        [SerializeField]
        private Slot curQuickSlot;
        public Slot CurQuickSlot { get { return curQuickSlot; } }

        [SerializeField]
        private Slot curInventorySlot;
        public Slot CurInventorySlot { get { return curInventorySlot; } }

        private void Awake()
        {
            owner = GetComponent<Player>();
        }

        // �����Կ� �Ҵ�� �����۰� ��ȣ�ۿ��ư ����
        public void ChangeButton()
        {
            ResetPrevButton(owner.curButtonMode);
            curImage?.SetActive(false);
            curItemModel?.SetActive(false);
            
            if (curQuickSlot.item == null || 
                curQuickSlot.item.itemdata == null || 
                curQuickSlot.item.itemdata.itemtype == ItemData.ItemType.Ingredient)
            {
                owner.curButtonMode = InteractButtonMode.None;
                return;
            }

            switch (curQuickSlot.item.itemdata.itemtype)
            {
                case ItemData.ItemType.Used:
                    potionImage.SetActive(true);
                    curImage = potionImage;
                    owner.curButtonMode = InteractButtonMode.Use;

                    // ��� ����
                    if(curQuickSlot.item.itemdata.name == "Meat" || curQuickSlot.item.itemdata.name == "NiceMeat")
                    {
                        curItemModel = curQuickSlot.item.itemdata.name == "Meat" ? meatModel : niceMeatModel;
                        curItemModel.SetActive(true);
                    }

                    break;
                case ItemData.ItemType.Equipment:
                    weaponImage.SetActive(true);
                    curImage = weaponImage;
                    Equip();
                    owner.curButtonMode = InteractButtonMode.Attack;
                    break;
                case ItemData.ItemType.Structure:
                    buildImage.SetActive(true);
                    curImage = buildImage;
                    owner.Builder.EnterBuildMode();
                    owner.curButtonMode = InteractButtonMode.Build;
                    break;
            }
        }

        // ���� ��ư ��Ȱ��ȭó��
        private void ResetPrevButton(InteractButtonMode prevMode)
        {
            switch (prevMode)
            {
                case InteractButtonMode.None:
                    break;
                case InteractButtonMode.Attack:
                    UnEquip(Equip_Item.EquipType.Weapon);
                    break;
                case InteractButtonMode.Build:
                    owner.Builder.ExitBuildMode();
                    break;
            }
        }

        // ������ ����
        public void Equip()
        {
            Slot curSlot = owner.IsOnBackpack ? curInventorySlot : curQuickSlot;
            if (curSlot == null) return;
            Equip_Item item = (Equip_Item)curSlot.item;
            if (item == null) return;

            // ���� ������ ��������
            UnEquip(item.equipType);
            switch (item.equipType)
            {
                // �߰� : ���� ������ ���� �ٸ���
                case Equip_Item.EquipType.Weapon:
                    curWeaponItem = item;
                    owner.Anim.SetBool("IsTwoHand", true);
                    SetEquipModel(curWeaponItem.atkType, curWeaponItem.Weapon_Level);
                    break;
                case Equip_Item.EquipType.Armor:
                    curArmorItem = item;
                    break;
            }
            // �� ������ ����
            item.Equip(owner);
        }
        // ������ ��������
        public void UnEquip(Equip_Item.EquipType type)
        {
            if (curWeaponItem == null) return;
            // ���� �� ����
            curWeaponModel?.SetActive(false);

            switch (type)
            {
                case Equip_Item.EquipType.Weapon:
                    curWeaponItem.UnEquip(owner);
                    owner.Anim.SetBool("IsPickAxe", false);
                    owner.Anim.SetBool("IsTwoHand", false);
                    break;
                case Equip_Item.EquipType.Armor:
                    curArmorItem.UnEquip(owner);
                    break;
            }
            curWeaponItem = null;
        }
        // ������ �� ����
        private void SetEquipModel(Equip_Item.ATKType atkType, int level = 0)
        {
            curWeaponModel?.SetActive(false);
            owner.Anim.SetBool("IsPickAxe", false);

            switch (atkType)
            {
                case Equip_Item.ATKType.Monster:
                    curWeaponModel = swordModel[level];
                    break;
                case Equip_Item.ATKType.Tree:
                    curWeaponModel = axeModel[level];
                    break;
                case Equip_Item.ATKType.Stone:
                    curWeaponModel = pickAxeModel[level];
                    owner.Anim.SetBool("IsPickAxe", true);
                    break;
            }
            curWeaponModel?.SetActive(true);
        }

        public void Use()
        {
            // ������ ���
            Slot curSlot = owner.IsOnBackpack ? curInventorySlot : curQuickSlot;
            Used_Item item = (Used_Item)curSlot.item;

            // �������� �������� �ʰų� �������� ������ ������� ����
            if (item == null ||
                curSlot.ItemCount < 1)
                return;
            // ������ ����ϴ� �������� �ƴ� ���
            if (!item.isInfinite)
            {
                curSlot.ItemCount--;

                owner.InteractableSource.clip = owner.InteractableClips[0];
                owner.InteractableSource.Play();

                // ����ϰ�� ��������
                if (curSlot.ItemCount < 1 && (curQuickSlot.item.itemdata.name == "Meat" || curQuickSlot.item.itemdata.name == "NiceMeat"))
                {
                    curItemModel.SetActive(false);
                    curItemModel = null;
                }
            }

            item.Use(owner);

            if ((curSlot == curQuickSlot) && curSlot.item == null || curSlot.itemCount < 1)
            {
                curImage?.SetActive(false);
            }
        }
        public void OnSelectQuickSlot(Slot slot)
        {
            // ���� ��������
            UnEquip(Equip_Item.EquipType.Weapon);

            curQuickSlot = slot;

            ChangeButton();
        }
        public void OnSelectInventorySlot(Slot slot)
        {
            curInventorySlot = slot;
        }
    }
}
