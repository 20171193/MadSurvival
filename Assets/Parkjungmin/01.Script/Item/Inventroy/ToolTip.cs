using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace jungmin
{
    public class ToolTip : MonoBehaviour
    {
        [SerializeField] GameObject use_Button;
        [SerializeField] GameObject destroy_Button;

        [SerializeField] Player player;

        public void Button_Use()
        {
            //switch(SelectedSlot_Inventory.instance.slot.item.itemdata.itemtype)//대분류 ItemType을 바탕으로 분류.
            //{
            //    case ItemData.ItemType.Equipment: //장비면
            //        //IsEquipItem();
            //        player.Equip((Equip_Item)SelectedSlot_Inventory.instance.slot.item);
            //        break;
            //    case ItemData.ItemType.Used: //소비 아이템이면
            //        //IsUsedItem();
            //        player.Use((Used_Item)SelectedSlot_Inventory.instance.slot.item);
            //        break;
            //    case ItemData.ItemType.ingredient: //재료 아이템이면
            //        break;
            //}
        }
        public void Button_Destroy()
        {
            SelectedSlot_Inventory.instance.SelectedSlot.ClearSlot();
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
        void IsUsedItem()
        {
            if (SelectedSlot_Inventory.instance.SelectedSlot.item is Used_Item)
            {
                //Used_Item used_Item = (Used_Item)SelectedSlot_Inventory.instance.SelectedSlot.item;

                //if (used_Item is HP_Potion)
                //{
                //    //(HP_Potion)used_Item.Use(Player player); // 플레이어를 가리키는 instance 없음
                //}
                //if (used_Item is Stamina_Potion)
                //{
                //    //(HP_Potion)used_Item.Use(Player player); 
                //}

            }
        }
    }
}
