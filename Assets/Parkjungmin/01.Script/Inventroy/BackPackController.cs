using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jungmin
{
	public class BackPackController : MonoBehaviour
	{
		public static bool inventory_Activated = false;
		[SerializeField] Slot[] slots;
		[SerializeField] GameObject Slot_parent;
		[SerializeField] GameObject inventory_Base;

		private void Start()
		{
			slots = Slot_parent.GetComponentsInChildren<Slot>();
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
			inventory_Base.SetActive(false);
		}
		public void AcquireItem(Item _item, int _count = 1) //아이템을 먹었을 때 인벤토리에 넣는 기능,자동으로 정렬되어 추가됨
		{
			if (_item.itemtype != Item.ItemType.Equipment) //획득 아이템 속성이 장비가 아니면
			{
				for (int i = 0; i < slots.Length; i++)
				{
					if (slots[i].item != null)
					{
						if (slots[i].item.itemName == _item.itemName) // 해당 아이템을 찾아 개수를 추가한다.
						{
							slots[i].SetSlotCount(_count);
							return;
						}
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



	}
}
