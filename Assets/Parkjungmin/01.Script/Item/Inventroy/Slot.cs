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
		[Header("아이템 정보")]
		public Item item;
        [Header("아이템의 현재 개수")]
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
        [Header("아이템의 내구도")]
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

		[SerializeField] public Image itemImage; //슬롯 위에 보여질 아이템 이미지.
		[SerializeField] TMP_Text text_Count;
		[SerializeField] GameObject go_CountImage;
		//[SerializeField] UnityAction OnToolTip;


		private void Start()
		{
			//OnToolTip += BackPackController.instance.TryOpenToolTip;
		}


		// **** Method : 새로운 아이템 추가 시, 그 슬롯의 UI를 업데이트하는 함수 ****
		public void AddItem(Item newItem, int newitemCount = 1)
		{ 
			// 1. 새로운 아이템과 기존의 아이템을 비교해, 슬롯에 아이템 정보와 갯수 설정
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

			// 2. Dic에서 아이템 이미지 불러오기
			if(newItem.itemdata.itemtype == ItemData.ItemType.Ingredient) //재료 속성인 경우 -> 재료 Dic
                itemImage.sprite = ItemManager.Instance.ingredientItemDic[newItem.itemdata.itemName].itemdata.itemImage;
            else														  //재료 속성이 아닌 경우 -> 크래프팅 Dic
				itemImage.sprite = ItemManager.Instance.craftingItemDic[newItem.itemdata.itemName].itemdata.itemImage;

			// 3. 슬롯의 아이템의 갯수UI Set On
			if (item.itemdata.itemtype != ItemData.ItemType.Equipment) //재료,소비,건설 타입 아이템 -> 갯수 UI On
			{
				go_CountImage.SetActive(true);
                text_Count.text = this.ItemCount.ToString();
			}
			else													  //장비 타입 아이템 ->  갯수 UI Off + 내구도 표시UI On
			{
				Equip_Item equip_Item = (Equip_Item)this.item;
				this.itemDurable = equip_Item.durable;
				text_Count.text = "0";
				go_CountImage.SetActive(false);
			}
			// 4.슬롯의 아이템 이미지 투명도 1로 변경.
			SetColor(1);
		}


        // Method : **** 슬롯의 아이템 갯수 업데이트 ****
        public void SetSlotCount(int _count)
		{
            ItemCount += _count;
			text_Count.text = ItemCount.ToString();

			if (ItemCount <= 0)
			{
				ClearSlot();
			}
		}


        // Method :  **** 슬롯 초기화 ****
        public void ClearSlot()
		{
			item = null;
            //ItemCount = 0;
			itemImage.sprite = GetComponent<Image>().sprite; //슬롯의 기본 스프라이트로 변경.
			SetColor(0);
			text_Count.text = "0";
			go_CountImage.SetActive(false);

		}
        // Method :  **** 슬롯의 아이템 이미지 투명도 변경 ****
        void SetColor(float alpha)
		{
			Color color = itemImage.color;
			color.a = alpha;
			itemImage.color = color;
		}
        // Method : **** 슬롯의 배경 이미지의 Red 값 변경 ****
        public void SetColorBG(float alpha)
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

			if (DragSlot.instance.dragSlot != null)
			{
				ChangeSlot();
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{

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
			//인벤토리가 꺼져있을 때만 반응하기에, 인벤토리와 무관.
			// 퀵슬롯에서 슬롯 선택하기
			if (!BackPackController.inventory_Activated)
			{
				if (SelectedSlot_QuickSlot.instance.SelectedSlot != null) //이전에 셀렉된 슬롯이 있었다면,
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
