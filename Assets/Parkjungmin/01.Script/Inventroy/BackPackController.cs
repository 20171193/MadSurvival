using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace jungmin
{
	public class BackPackController : MonoBehaviour
	{
		public static bool inventory_Activated = false;
		public static BackPackController instance;
		[SerializeField] public Slot[] slots;
		[SerializeField] GameObject Slot_parent;
		[SerializeField] GameObject inventory_Base;
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
			inventory_Base.SetActive(false);
		}
		public void AcquireItem(ItemData _item, int _count = 1) //�������� �Ծ��� �� �κ��丮�� �ִ� ���,�ڵ����� ���ĵǾ� �߰���
		{
			if (_item.itemtype != ItemData.ItemType.Equipment) //ȹ�� ������ �Ӽ��� ��� �ƴϸ�
			{
				for (int i = 0; i < slots.Length; i++)
				{
					if (slots[i].item != null && (slots[i].item.itemName == _item.itemName)) // ���� ������ �ִ� ������ �߰����� ��.
					{
						if (slots[i].item.itemName == _item.itemName) // �ش� �������� ã�� ������ �߰��Ѵ�.
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
			for (int i = 0; i < slots.Length; i++) // ȹ�� ������ �Ӽ��� ����
			{
				if (slots[i].item == null) // �� ������ ã�� �׳� �ִ´�.
				{
					slots[i].AddItem(_item, _count);
					return;
				}
			}
		}
		public void LoseItem(ItemData _item, int _count = 1) //�������� �Ұ���. 
		{
			if(_item.itemtype != ItemData.ItemType.Equipment)
			{
				for(int i=0;i<slots.Length;i++)
				{
                    if (slots[i].item != null && (slots[i].item.itemName == _item.itemName)) // ���� ������ �ִ� ������ �߰����� ��.
                    {
                        if (slots[i].item.itemName == _item.itemName) // �ش� �������� ã�� ������ ���ҽ�Ų��.
                        {
                            slots[i].SetSlotCount(-_count);
                            return;
                        }
                    }
                }
			}
            for (int i = 0; i < slots.Length; i++) // ȹ�� ������ �Ӽ��� ����
            {
                if (slots[i].item != null && (slots[i].item.itemName == _item.itemName)) // �� ������ ã�� �׳� �ִ´�.
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



	}
}
