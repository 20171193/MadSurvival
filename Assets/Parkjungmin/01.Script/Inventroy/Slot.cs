using jungmin;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Jc;
using System.Linq;
namespace jungmin
{
	public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
	{
		[Header("������ ����")]
		public Item item;
        [Header("�������� ���� ����")]
        public int itemCount;
		public int ItemCount
		{
			get { return this.itemCount; }
			set
			{
				this.itemCount = value;

			}
		}
        [Header("�������� ������")]
        public int itemDurable;
		public int ItemDurable
		{
			get { return itemDurable; }
			set
			{
				itemCount = value;
			}
		}

		[SerializeField] public Image itemImage; //���� ���� ������ ������ �̹���.
		[SerializeField] TMP_Text text_Count;
		[SerializeField] GameObject go_CountImage;


		// **** Method : ���ο� ������ �߰� ��, �� ������ UI�� ������Ʈ�ϴ� �Լ� ****
		public void AddItem(Item newItem, int newitemCount = 1)
		{ 
			// 1. ���ο� �����۰� ������ �������� ����, ���Կ� ������ ������ ���� ����
			if (item != null && newItem.itemdata.itemName != this.item.itemdata.itemName)
			{ 
                this.item = newItem;
                this.ItemCount = newitemCount;
			}
			else 
			{				
                this.item = newItem;
                this.ItemCount += newitemCount;
			}

			// 2. Dic���� ������ �̹��� �ҷ�����
			if (ItemManager.Instance.ingredientItemDic.ContainsKey($"{newItem.itemdata.itemName}"))
			{
                // �������� �̸��� Dic���� �˻��� �ش�Ǵ� �����Ϳ� ����� �������� �̹����� �����´�.
                itemImage.sprite = ItemManager.Instance.ingredientItemDic[newItem.itemdata.itemName].itemdata.itemImage;
			}
			else
				itemImage.sprite = ItemManager.Instance.craftingItemDic[newItem.itemdata.itemName].itemdata.itemImage;

			// 3. ������ �������� ����UI Set On
			if (item.itemdata.itemtype != ItemData.ItemType.Equipment) //���,�Һ�,�Ǽ� Ÿ�� ������ -> ���� UI On
			{
				if (item is Bottle)
				{
					Bottle bottle = (Bottle)item;
					// ���� �뷮 ������Ʈ ���
					bottle.OnUseBottle += UpdateSlotCount;
					text_Count.text = bottle.ownCapacity.ToString();
					Debug.Log($"������ �� {bottle.ownCapacity}");
					go_CountImage.SetActive(true);
					//UpdateSlotCount();
				}
				else
				{
					go_CountImage.SetActive(true);
					text_Count.text = this.ItemCount.ToString();
				}
			}
			else													  //��� Ÿ�� ������ ->  ���� UI Off + ������ ǥ��UI On
			{
				Equip_Item equipItem = (Equip_Item)this.item;
				this.itemDurable = equipItem.durable;
                go_CountImage.SetActive(true);
				text_Count.text = $"{equipItem.durable}%";
				equipItem.OnUse += UpdateSlotCount;
                //go_CountImage.SetActive(false);
            }
			// 4.������ ������ �̹��� ���� 1�� ����.
			SetColor(1);
		}


        // Method : **** ������ ������ ������ ������ �߰�. ****
        public void SetSlotCount(int _count)
		{
            ItemCount += _count;
			text_Count.text = ItemCount.ToString();

			if (ItemCount <= 0)
			{
				ClearSlot();
			}
		}
		// Method : **** ������ ������ ���� UI�� ���� ������ �°� ������Ʈ ****
		public void UpdateSlotCount()
		{
			if (item is Bottle) //���� �������̸�
			{
				Bottle bottle = (Bottle)item;
				text_Count.text = bottle.ownCapacity.ToString();

			}
			else if(item is Equip_Item) //�������� �ִ� ��� �������̸�
			{
				Equip_Item equipItem = (Equip_Item)item;
				text_Count.text = $"{equipItem.durable}%";
			}
			else
			{
				text_Count.text = ItemCount.ToString();
			}

            if (ItemCount <= 0)
            {
                ClearSlot();
            }
        }


        // Method :  **** ���� �ʱ�ȭ ****
        public void ClearSlot()
		{
			if(item is Bottle)
			{
				Bottle bottle = (Bottle)item;
				bottle.OnUseBottle -= UpdateSlotCount;
			}
			item = null;
            ItemCount = 0;
			itemImage.sprite = GetComponent<Image>().sprite; //������ �⺻ ��������Ʈ�� ����.
			SetColor(0);
			text_Count.text = "0";
			go_CountImage.SetActive(false);

		}
        // Method :  **** ������ ������ �̹��� ���� ���� ****
        void SetColor(float alpha)
		{
			Color color = itemImage.color;
			color.a = alpha;
			itemImage.color = color;
		}
        // Method : **** ������ ��� �̹����� Red �� ���� ****
        public void SetColorBG(float alpha)
		{
			Color color = GetComponent<Image>().color;
			color.r = alpha;
			GetComponent<Image>().color = color;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			// Ŭ���� ���� �������� �����ϴ���?
			if (BackPackController.inventory_Activated)
			{
				// ���ѿ� �������� �����ϴ� ���
				if (item != null)
				{
					DragSlot.instance.transform.position = eventData.position;
					DragSlot.instance.dragSlot = this;
					DragSlot.instance.DragSetImage(itemImage);
				}
			}
		}
		public void OnDrag(PointerEventData eventData)
		{
			// �̹��� �巡��
			if (BackPackController.inventory_Activated)
			{
				if (item != null)
				{
					DragSlot.instance.transform.position = eventData.position;
				}
			}
		}

		// Method : ��� ���� ������ �巡�� �� ��� �̺�Ʈ �� �� ���Կ��� ȣ�� ****
		public void OnDrop(PointerEventData eventData)
		{

			if(QuickSlotController.instance.slots.Contains(this))
			{
				foreach (Slot slot in QuickSlotController.instance.slots)
				{
					//Debug.Log("?");
					slot.SetColorBG(255);
				}
				SelectedSlot_QuickSlot.instance.selectedSlot = this;
				SelectSlot_QuickSlot();

            }
			if (DragSlot.instance.dragSlot != null)
			{
                foreach (Slot slot in BackPackController.instance.slots)
                {
                    slot.SetColorBG(255);
                }
                ChangeSlot();
			}
		}
        // Method : ���� ���� ��ȯ ****
        void ChangeSlot()
        {
            // 1. OnDrop �̺�Ʈ�� ȣ��� ������ �����͵��� �̸� �����صд�.
            Item tempItem = item;
            int tempItemCount = ItemCount;

            // 2.������ ���Կ� �ִ� �������� ������ 0���� ����
            this.ItemCount = 0;

            // 3.  OnDrop �̺�Ʈ�� ȣ��� ���Կ� �巡�� ���� �������� �߰�.
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.ItemCount);
			
			// 4. ���� ������ �������� ���� ������ ���� ���� ������ ������Ʈ�Ѵ�.
            if (tempItem != null)
            {					  
                DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);

				if(tempItem is Bottle) //4.1 ���� ���� �������̾��ٸ�, ������ �̺�Ʈ�� �����Ѵ�.
				{
					Bottle bottle = (Bottle)tempItem;
					bottle.OnUseBottle -= UpdateSlotCount;

                }
				
            }
            else
            { 
                DragSlot.instance.dragSlot.ClearSlot();

            }
        }

        public void OnEndDrag(PointerEventData eventData)
		{

			// �巡������ �̹��� ����
			if (BackPackController.inventory_Activated)
			{
				DragSlot.instance.SetColor(0);
				DragSlot.instance.dragSlot = null;

			}
		}
        // Method : �����Կ��� ���� �����ϱ� ****
        public void SelectSlot_QuickSlot()
		{

			if (SelectedSlot_QuickSlot.instance.SelectedSlot != null) //������ ������ ������ �־��ٸ�,
			{
				SelectedSlot_QuickSlot.instance.SelectedSlot.SetColorBG(255);
				SelectedSlot_QuickSlot.instance.SelectedSlot = this;
				SetColorBG(0);
			}
			else
			{
				SelectedSlot_QuickSlot.instance.SelectedSlot = this;
				SetColorBG(0);
			}

		}
        // Method : �κ��丮���� ���� �����ϱ� ****
        void SelectSlot_Inventory()
		{
            // 1. �������� ���õ� ������ ���� �̹����� �������� �򰥸��� �ʰ� �Ѵ�.
            if (SelectedSlot_QuickSlot.instance.SelectedSlot != null)
            {
                SelectedSlot_QuickSlot.instance.SelectedSlot.SetColorBG(255);
            }
			// 2. ���õ� ������ ��� �̹����� Red ���� �ٲ㼭 ���õ� ���� Ȯ�ν�Ų��.
			if (SelectedSlot_Inventory.instance.SelectedSlot != null)
			{
				SelectedSlot_Inventory.instance.SelectedSlot.SetColorBG(255);
				SelectedSlot_Inventory.instance.SelectedSlot = this;
				SetColorBG(0);
			}
			else
			{
				SelectedSlot_Inventory.instance.SelectedSlot = this;
				SetColorBG(0);
			}

		}

		public void OnPointerClick(PointerEventData eventData)
		{

			if(item == null && BackPackController.instance.slots.Contains(this))
			{
				return;
			}
			if (BackPackController.inventory_Activated && BackPackController.instance.slots.Contains(this)) 
			{
				SelectSlot_Inventory();
				if(SelectedSlot_Inventory.instance.SelectedSlot.item != null)
				{
					BackPackController.instance.TryOpenToolTip();
                }
			}
			else
			{
				SelectSlot_QuickSlot();
			}
		}
	}
}
