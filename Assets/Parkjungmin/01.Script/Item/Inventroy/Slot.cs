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
		Vector3 originPos;
		public Item item;
		public int itemCount;
		[SerializeField] public Image itemImage; //���� ���� ������ ������ �̹���.
		[SerializeField] TMP_Text text_Count;
		[SerializeField] GameObject go_CountImage;
		//[SerializeField] UnityAction OnToolTip;


		private void Start()
		{
            originPos = transform.position;
			//OnToolTip += BackPackController.instance.TryOpenToolTip;
		}
		public void AddItem(Item _item, int itemCount = 1) //���� ���� ������ UI ������Ʈ
		{
			item = _item;
			this.itemCount = itemCount;

			itemImage.sprite = ItemManager.Instance.craftingItemDic[_item.itemdata.itemName].itemdata.itemImage;

			if (item.itemdata.itemtype != ItemData.ItemType.Equipment) //ü��,�Һ� �������̸�,
			{
				go_CountImage.SetActive(true);
				text_Count.text = this.itemCount.ToString();
			}
			else
			{
				text_Count.text = "0";
				go_CountImage.SetActive(false);
			}

			SetColor(1);
		}
		public void SetSlotCount(int _count)
		{
			itemCount += _count;
			text_Count.text = itemCount.ToString();

			if (itemCount <= 0)
			{
				ClearSlot();
			}
		}
		public void ClearSlot()
		{
			item = null;
			itemCount = 0;
			itemImage.sprite = GetComponent<Image>().sprite; //������ �⺻ ��������Ʈ�� ����.
			SetColor(0);
			text_Count.text = "0";
			go_CountImage.SetActive(false);

		}
		void SetColor(float alpha) //������ ������ �̹����� ������ ����
		{
			Color color = itemImage.color;
			color.a = alpha;
			itemImage.color = color;
		}
		void SetColorBG(float alpha) //������ Ʋ �̹����� ������ ����
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
			Debug.Log($"End {this}");

			if (DragSlot.instance.dragSlot != null)
			{
				ChangeSlot();
			}
			else
			{
				Debug.Log("DragSlot.instance.dragSlot is null");
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			Debug.Log($"Start {this}");

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
			int tempItemCount = itemCount;

			AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
			if (tempItem != null)
			{
				DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
			}
			else
			{

				DragSlot.instance.dragSlot.ClearSlot();
			}
		}
		void SelectSlot_QuickSlot()
		{
			//�κ��丮�� �������� ���� �����ϱ⿡, �κ��丮�� ����.
			// �����Կ��� ���� �����ϱ�
			if (!BackPackController.inventory_Activated)
			{
				if (SelectedSlot_QuickSlot.instance.SelectedSlot != null) //������ ������ ������ �־��ٸ�,
				{
					SelectedSlot_QuickSlot.instance.SelectedSlot.SetColorBG(255);
					Debug.Log("SelectSlot ��ü ����");
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
