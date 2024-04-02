using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPackController: MonoBehaviour
{
	public static bool isActivated = false;
	[SerializeField] Slot[] slots;
	[SerializeField] GameObject Slot_parent;
	[SerializeField] GameObject inventoryBase;

	private void Start()
	{
		slots = Slot_parent.GetComponentsInChildren<Slot>();
	}
	public void TryOpenInventory()
	{

		isActivated = !isActivated;

		if ( isActivated )
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
		inventoryBase.SetActive(true);
	}
	void CloseInventory()
	{
		inventoryBase.SetActive(false);
	}
	public void AcquireItem(Item _item,int _count = 1)
	{
		if(_item.itemtype != Item.ItemType.Equipment)
		{
			for(int i=0;i<slots.Length;i++ )
			{
				if(slots[i].item != null)
				{ 
					if (slots[i].item.itemName == _item.itemName )
					{
						slots [i].SetSlotCount(_count);
						return;
					}
				}
			}
		}
			for ( int i = 0; i < slots.Length; i++ )
			{
				if (slots[i].item == null)
				{
					slots [i].AddItem(_item, _count);
					return;
				}
			}
	}
}
