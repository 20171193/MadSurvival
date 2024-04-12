using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;


namespace jungmin
{
	public class BackPackController : MonoBehaviour
	{
		public static bool inventory_Activated = false;
		public static bool tooltip_Activated = false;
		public static BackPackController instance;
		[SerializeField] public Slot[] slots;
		[SerializeField] GameObject Slot_parent;
		[SerializeField] GameObject inventory_Base;
		[SerializeField] GameObject toolTip;
		UnityAction OnCraft;

		private void Start()
		{
			instance = this;
			slots = Slot_parent.GetComponentsInChildren<Slot>();
			OnCraft += ItemManager.Instance.CraftItem;
		}
		public void TryOpenInventory()
		{

			inventory_Activated = !inventory_Activated;

			if (inventory_Activated)
			{
				OpenInventory();
			}
			else
			{
				CloseInventory();
			}

		}
		void OpenInventory()
		{
			inventory_Base.SetActive(true);
		}
		void CloseInventory()
		{
			if(toolTip.activeSelf == true) { TryOpenToolTip(); }
			inventory_Base.SetActive(false);
            // 1.인벤토리를 닫을 때는 인벤토리에서 선택된 슬롯을 Null로 바꾼다.
            if (SelectedSlot_Inventory.instance.SelectedSlot != null)
            {
				SelectedSlot_Inventory.instance.SelectedSlot = null;
            }
        }
		public void AcquireItem(Item _item, int _count = 1) //아이템을 먹었을 때 인벤토리에 넣는 기능,자동으로 정렬되어 추가됨
		{
			if (_item.itemdata.itemtype != ItemData.ItemType.Equipment) //획득 아이템 속성이 장비가 아니면
			{
				for (int i = 0; i < slots.Length; i++)
				{
					if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // 같은 아이템 있는 슬롯을 발견했을 때.
					{
						if (slots[i].item.itemdata.itemName == _item.itemdata.itemName) // 해당 아이템을 찾아 개수를 추가한다.
						{
                            slots[i].SetSlotCount(_count);
							return;
						}
					}
                    else if (slots[i].item == null) 
                    {
                        slots[i].AddItem(_item, _count);
                        return;
                    }
                }
			}
			for (int i = 0; i < slots.Length; i++) // 획득 아이템 속성이 장비면
			{
				if (slots[i].item == null) // 빈 슬롯을 찾아 그냥 넣는다.
				{
					slots[i].AddItem(_item, _count);
					return;
				}
			}
		}
		public void LoseItem(Item _item, int _count = 1) //아이템을 잃게함. 
		{
			if(_item.itemdata.itemtype != ItemData.ItemType.Equipment)
			{
				for(int i=0;i<slots.Length;i++)
				{
                    if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // 같은 아이템 있는 슬롯을 발견했을 때.
                    {
                        if (slots[i].item.itemdata.itemName == _item.itemdata.itemName) // 해당 아이템을 찾아 개수를 감소시킨다.
                        {
                            slots[i].SetSlotCount(-_count);
                            return;
                        }
                    }
                }
			}
            for (int i = 0; i < slots.Length; i++) // 획득 아이템 속성이 장비면
            {
                if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // 빈 슬롯을 찾아 그냥 넣는다.
                {
					slots[i].ClearSlot();
                    return;
                }
            }
        }

        public void ButtonToCraft()
		{
            OnCraft?.Invoke();
		}



        public void TryOpenToolTip()
        {

			tooltip_Activated = !tooltip_Activated;

            if (tooltip_Activated)
            {
                OpenToolTip();
            }
            else
            {
                CloseToolTip();
            }

        }
        public void OpenToolTip()
        {
			toolTip.SetActive(true);
        }
        public void CloseToolTip()
        {
			toolTip.SetActive(false);
        }

    }
}
