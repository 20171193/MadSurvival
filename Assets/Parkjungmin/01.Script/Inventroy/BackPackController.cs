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
            // 1.�κ��丮�� ���� ���� �κ��丮���� ���õ� ������ Null�� �ٲ۴�.
            if (SelectedSlot_Inventory.instance.SelectedSlot != null)
            {
				SelectedSlot_Inventory.instance.SelectedSlot = null;
            }
        }
		public void AcquireItem(Item _item, int _count = 1) //�������� �Ծ��� �� �κ��丮�� �ִ� ���,�ڵ����� ���ĵǾ� �߰���
		{
			if (_item.itemdata.itemtype != ItemData.ItemType.Equipment) //ȹ�� ������ �Ӽ��� ��� �ƴϸ�
			{
				for (int i = 0; i < slots.Length; i++)
				{
					if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // ���� ������ �ִ� ������ �߰����� ��.
					{
						if (slots[i].item.itemdata.itemName == _item.itemdata.itemName) // �ش� �������� ã�� ������ �߰��Ѵ�.
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
		public void LoseItem(Item _item, int _count = 1) //�������� �Ұ���. 
		{
			if(_item.itemdata.itemtype != ItemData.ItemType.Equipment)
			{
				for(int i=0;i<slots.Length;i++)
				{
                    if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // ���� ������ �ִ� ������ �߰����� ��.
                    {
                        if (slots[i].item.itemdata.itemName == _item.itemdata.itemName) // �ش� �������� ã�� ������ ���ҽ�Ų��.
                        {
                            slots[i].SetSlotCount(-_count);
                            return;
                        }
                    }
                }
			}
            for (int i = 0; i < slots.Length; i++) // ȹ�� ������ �Ӽ��� ����
            {
                if (slots[i].item != null && (slots[i].item.itemdata.itemName == _item.itemdata.itemName)) // �� ������ ã�� �׳� �ִ´�.
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
