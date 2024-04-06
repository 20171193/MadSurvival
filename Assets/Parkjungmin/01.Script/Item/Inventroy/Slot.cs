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
		[SerializeField] public Image itemImage; //슬롯 위에 보여질 아이템 이미지.
		[SerializeField] TMP_Text text_Count;
		[SerializeField] GameObject go_CountImage;
		//[SerializeField] UnityAction OnToolTip;


		private void Start()
		{
            originPos = transform.position;
			//OnToolTip += BackPackController.instance.TryOpenToolTip;
		}
		public void AddItem(Item _item, int itemCount = 1) //슬롯 상의 아이템 UI 업데이트
		{
			item = _item;
			this.itemCount = itemCount;

			itemImage.sprite = ItemManager.Instance.craftingItemDic[_item.itemdata.itemName].itemdata.itemImage;

			if (item.itemdata.itemtype != ItemData.ItemType.Equipment) //체력,소비 아이템이면,
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
			itemImage.sprite = GetComponent<Image>().sprite; //슬롯의 기본 스프라이트로 변경.
			SetColor(0);
			text_Count.text = "0";
			go_CountImage.SetActive(false);

		}
		void SetColor(float alpha) //슬롯의 아이템 이미지의 색깔을 변경
		{
			Color color = itemImage.color;
			color.a = alpha;
			itemImage.color = color;
		}
		void SetColorBG(float alpha) //슬롯의 틀 이미지의 색깔을 변경
		{
			Color color = GetComponent<Image>().color;
			color.r = alpha;
			GetComponent<Image>().color = color;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			// 클릭한 곳에 아이템이 존재하는지?
			//if ()
			if (BackPackController.inventory_Activated)
			{
				// 백팩에 아이템이 존재하는 경우
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
			// 이미지 드래그
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

			// 드래그중인 이미지 리턴
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
			//인벤토리가 꺼져있을 때만 반응하기에, 인벤토리와 무관.
			// 퀵슬롯에서 슬롯 선택하기
			if (!BackPackController.inventory_Activated)
			{
				if (SelectedSlot_QuickSlot.instance.SelectedSlot != null) //이전에 셀렉된 슬롯이 있었다면,
				{
					SelectedSlot_QuickSlot.instance.SelectedSlot.SetColorBG(255);
					Debug.Log("SelectSlot 교체 실행");
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
				if (SelectedSlot_Inventory.instance.SelectedSlot != null) //이전에 셀렉된 슬롯이 있었다면,
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
			/* 인벤토리가 켜져있을 땐 툴팁 On / 사용 버리기 선택.
			 * 인벤토리가 꺼져있을 땐 퀵슬롯의 슬롯 선택
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
				Debug.Log("현재 그 슬롯에 아이템이 없습니다.");
			}
			else
			{
				SelectSlot_QuickSlot();
			}
		}
	}
}
