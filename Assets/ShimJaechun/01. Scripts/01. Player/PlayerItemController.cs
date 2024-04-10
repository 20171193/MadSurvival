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
        private GameObject monsterWeaponModel;
        [SerializeField]
        private GameObject treeWeaponModel;
        [SerializeField]
        private GameObject stoneWeaponModel;
        private GameObject curWeaponModel;

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
        public Slot CurQuickSlot { get { return curQuickSlot; }  }

        [SerializeField]
        private Slot curInventorySlot;
        public Slot CurInventorySlot { get { return curInventorySlot; } }

        private void Awake()
        {
            owner = GetComponent<Player>();
        }

        public void ChangeButton()
        {
            if (curQuickSlot == null) return;
            if (curQuickSlot.item == null) return;

            //isBuildMode = false;
            curImage?.SetActive(false);

            switch (curQuickSlot.item.itemdata.itemtype)
            {
                case ItemData.ItemType.Used:
                    potionImage.SetActive(true);
                    curImage = potionImage;
                    owner.curButtonMode = InteractButtonMode.Use;
                    break;
                case ItemData.ItemType.Equipment:
                    weaponImage.SetActive(true);
                    curImage = weaponImage;
                    break;
                case ItemData.ItemType.Structure:
                    buildImage.SetActive(true);
                    curImage = buildImage;
                    owner.Builder.EnterBuildMode();
                    break;
            }

        }

        // ������ ����
        public void Equip()
        {
            Slot curSlot = owner.IsOnBackpack ? curInventorySlot : curQuickSlot;

            Equip_Item item = (Equip_Item)curSlot.item;
            if (item == null) return;

            // ���� ������ ��������
            UnEquip(item.equipType);
            switch (item.equipType)
            {
                case Equip_Item.EquipType.Weapon:
                    curWeaponItem = item;
                    owner.Anim.SetBool("IsTwoHand", true);
                    SetEquipModel(curWeaponItem.atkType);
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
                    owner.Anim.SetBool("IsTwoHand", false);
                    break;
                case Equip_Item.EquipType.Armor:
                    curArmorItem.UnEquip(owner);
                    break;
            }
            curWeaponItem = null;
        }
        // ������ �� ����
        private void SetEquipModel(Equip_Item.ATKType atkType)
        {
            curWeaponModel?.SetActive(false);

            switch (atkType)
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

        public void Use()
        {
            // ������ ���
            Slot curSlot = owner.IsOnBackpack ? curInventorySlot : curQuickSlot;
            Used_Item item = (Used_Item)curSlot.item;
            if (item == null ||
                curSlot.ItemCount < 1) return;

            curSlot.ItemCount--;
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
