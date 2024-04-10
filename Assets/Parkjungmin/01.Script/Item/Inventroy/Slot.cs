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
			get { return itemCount; }
			set
			{
				itemCount = value;

				text_Count.text = itemCount.ToString();

				if (itemCount <= 0)
				{
					ClearSlot();
				}
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

				if (this.itemDurable <= 0)
				{
					ClearSlot();
				}
			}
		}

		[SerializeField] public Image itemImage; //���� ���� ������ ������ �̹���.
		[SerializeField] TMP_Text text_Count;
		[SerializeField] GameObject go_CountImage;
		//[SerializeField] UnityAction OnToolTip;


		private void Start()
		{
			//OnToolTip += BackPackController.instance.TryOpenToolTip;
		}


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
			if(newItem.itemdata.itemtype == ItemData.ItemType.Ingredient) //��� �Ӽ��� ��� -> ��� Dic
                itemImage.sprite = ItemManager.Instance.ingredientItemDic[newItem.itemdata.itemName].itemdata.itemImage;
            else														  //��� �Ӽ��� �ƴ� ��� -> ũ������ Dic
				itemImage.sprite = ItemManager.Instance.craftingItemDic[newItem.itemdata.itemName].itemdata.itemImage;

			// 3. ������ �������� ����UI Set On
			if (item.itemdata.itemtype != ItemData.ItemType.Equipment) //���,�Һ�,�Ǽ� Ÿ�� ������ -> ���� UI On
			{
				go_CountImage.SetActive(true);
                text_Count.text = this.ItemCount.ToString();
			}
			else													  //��� Ÿ�� ������ ->  ���� UI Off + ������ ǥ��UI On
			{
				Equip_Item equip_Item = (Equip_Item)this.item;
				this.itemDurable = equip_Item.durable;
				text_Count.text = "0";
				go_CountImage.SetActive(false);
			}
			// 4.������ ������ �̹��� ���� 1�� ����.
			SetColor(1);
		}


        // Method : **** ������ ������ ���� ������Ʈ ****
        public void SetSlotCount(int _count)
		{
            ItemCount += _count;
			text_Count.text = ItemCount.ToString();

			if (ItemCount <= 0)
			{
				ClearSlot();
			}
		}


        // Method :  **** ���� �ʱ�ȭ ****
        public void ClearSlot()
		{
			item = null;
            //ItemCount = 0;
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
			//if ()
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
		public void OnDrop(PointerEventData eventData)
		{

			if (DragSlot.instance.dragSlot != null)
			{
				ChangeSlot();
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
		void ChangeSlot()
		{
			Item tempItem = item;
			int tempItemCount = ItemCount;

			AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.ItemCount);

			if (tempItem != null)
			{
				DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
			}
			else
			{ //

				DragSlot.instance.dragSlot.ClearSlot();
			}
		}
		public void SelectSlot_QuickSlot()
		{
			//�κ��丮�� �������� ���� �����ϱ⿡, �κ��丮�� ����.
			// �����Կ��� ���� �����ϱ�
			if (!BackPackController.inventory_Activated)
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
        }
		void SelectSlot_Inventory()
		{
			if (BackPackController.inventory_Activated)
			{
				if (SelectedSlot_Inventory.instance.SelectedSlot != null) //������ ������ ������ �־��ٸ�,
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
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			/* �κ��丮�� �������� �� ���� On / ��� ������ ����.
			 * �κ��丮�� �������� �� �������� ���� ����
			 * 
			 */
			//Debug.Log("OnPointerClick event");
			if (item != null && BackPackController.inventory_Activated)
			{
				SelectSlot_Inventory();
				//OnToolTip?.Invoke();
			}
			else if (item == null)
			{
				Debug.Log("���� �� ���Կ� �������� �����ϴ�.");
			}
			else
			{
				SelectSlot_QuickSlot();
			}
		}
	}
}
