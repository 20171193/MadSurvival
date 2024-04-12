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



        //Method : **** 활성화 되었을 때, 툴팁의 필드에 현재 아이템 데이터 할당 ****
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

        //Method : **** 비활성화 되었을 때, 툴팁의 모든 데이터를 초기화 ****
        private void OnDisable()
        {
            this.itemImage.sprite = null;
            this.itemName.text = "";
            this.item_info.text = "";
        }

        // Method : ****    사용 버튼   ****
        public void Button_Use()
        {
            // 1.플레이어에서 Use() 메소드 사용
            playerItemController.Use();
            // 2.사용 후 슬롯의 개수 UI 업데이트 
            SelectedSlot_Inventory.instance.SelectedSlot.UpdateSlotCount();
            // 3.만일 사용 후 갯수가 0이 되었다면 Use,Destroy 버튼 비활성화.
            if(SelectedSlot_Inventory.instance.SelectedSlot.itemCount <= 0)
            {
                use_Button.SetActive(false);
                destroy_Button.SetActive(false);
            }
        }


        // Method : ****    제거 버튼   ****
        public void Button_Destroy()
        {
            SelectedSlot_Inventory.instance.SelectedSlot.ClearSlot();
            gameObject.SetActive(false);
        }


        // Method : ****    취소 버튼   ****
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
