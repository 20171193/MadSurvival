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
		public void AcquireItem(Item _item, int _count = 1)
		{
			if (_item.itemtype != Item.ItemType.Equipment)
			{
				for (int i = 0; i < slots.Length; i++)
				{
					if (slots[i].item != null)
					{
						if (slots[i].item.itemName == _item.itemName)
						{
							slots[i].SetSlotCount(_count);
							return;
						}
					}
				}
			}
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i].item == null)
				{
					slots[i].AddItem(_item, _count);
					return;
				}
			}
		}
	}
}
