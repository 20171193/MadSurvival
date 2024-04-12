using Jc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace jungmin
{
    public class ToolTip : MonoBehaviour
    {
         bool isToolTipActivated;
         static ToolTip instance;
         public ToolTip Instance { get { return instance; } }
        
        [SerializeField] Image itemImage; 
        [SerializeField] GameObject use_Button;
        [SerializeField] GameObject destroy_Button;
        [SerializeField] TMP_Text itemName;
        [SerializeField] TMP_Text item_info;
        [SerializeField] PlayerItemController playerItemController; 

        public void Start()
        {
            instance = this;
        }



        //Method : **** Ȱ��ȭ �Ǿ��� ��, ������ �ʵ忡 ���� ������ ������ �Ҵ� ****
        private void OnEnable()
        {
            if(destroy_Button.activeSelf == false)
            {
                destroy_Button.SetActive(true);
            }

            if (SelectedSlot_Inventory.instance.SelectedSlot.item.itemdata.itemtype != ItemData.ItemType.Used)
            {
                use_Button.SetActive(false);

            }
            else
            {
                use_Button.SetActive(true);
            }

            this.itemImage.sprite = SelectedSlot_Inventory.instance.SelectedSlot.itemImage.sprite;
            this.itemName.text = SelectedSlot_Inventory.instance.SelectedSlot.item.itemdata.itemName;
            if(SelectedSlot_Inventory.instance.SelectedSlot.item.itemdata.itemInfo != "")
            { this.item_info.text = SelectedSlot_Inventory.instance.SelectedSlot.item.itemdata.itemInfo.ToString(); }
        }

        //Method : **** ��Ȱ��ȭ �Ǿ��� ��, ������ ��� �����͸� �ʱ�ȭ ****
        private void OnDisable()
        {
            this.itemImage.sprite = null;
            this.itemName.text = "";
            this.item_info.text = "";
        }

        // Method : ****    ��� ��ư   ****
        public void Button_Use()
        {
            // 1.�÷��̾�� Use() �޼ҵ� ���
            playerItemController.Use();
            // 2.��� �� ������ ���� UI ������Ʈ 
            SelectedSlot_Inventory.instance.SelectedSlot.UpdateSlotCount();
            // 3.���� ��� �� ������ 0�� �Ǿ��ٸ� Use,Destroy ��ư ��Ȱ��ȭ.
            if(SelectedSlot_Inventory.instance.SelectedSlot.itemCount <= 0)
            {
                use_Button.SetActive(false);
                destroy_Button.SetActive(false);
            }
        }


        // Method : ****    ���� ��ư   ****
        public void Button_Destroy()
        {
            SelectedSlot_Inventory.instance.SelectedSlot.ClearSlot();
            gameObject.SetActive(false);
        }


        // Method : ****    ��� ��ư   ****
        public void Button_Cancel()
        {
            transform.gameObject.SetActive(false);
        }
        void IsEquipItem()
        {
            if (SelectedSlot_Inventory.instance.SelectedSlot.item is Equip_Item)
            {
                Equip_Item equip_Item = (Equip_Item)SelectedSlot_Inventory.instance.SelectedSlot.item;

                if (equip_Item is Sword)
                {
                    //   (Sword)(equip_Item).Equip(Player);
                }
                if (equip_Item is Axe)
                {
                    //  (Axe)(equip_Item).Equip(Player);
                }
                if (equip_Item is PickAxe)
                {
                    // (PickAxe)(equip_Item).Equip(Player);
                }
            }
        }
    }
}
