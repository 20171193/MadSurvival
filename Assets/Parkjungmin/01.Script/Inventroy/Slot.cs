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
		[Header("아이템 정보")]
		public Item item;
        [Header("아이템의 현재 개수")]
        public int itemCount;
		public int ItemCount
		{
			get { return this.itemCount; }
			set
			{
				this.itemCount = value;

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
			}
		}

		[SerializeField] public Image itemImage; //슬롯 위에 보여질 아이템 이미지.
		[SerializeField] TMP_Text text_Count;
		[SerializeField] GameObject go_CountImage;


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
			if (ItemManager.Instance.ingredientItemDic.ContainsKey($"{newItem.itemdata.itemName}"))
			{
                // 아이템의 이름을 Dic에서 검색해 해당되는 데이터에 저장된 아이템의 이미지를 가져온다.
                itemImage.sprite = ItemManager.Instance.ingredientItemDic[newItem.itemdata.itemName].itemdata.itemImage;
			}
			else
				itemImage.sprite = ItemManager.Instance.craftingItemDic[newItem.itemdata.itemName].itemdata.itemImage;

			// 3. 슬롯의 아이템의 갯수UI Set On
			if (item.itemdata.itemtype != ItemData.ItemType.Equipment) //재료,소비,건설 타입 아이템 -> 갯수 UI On
			{
				if (item is Bottle)
				{
					Bottle bottle = (Bottle)item;
					// 물병 용량 업데이트 등록
					bottle.OnUseBottle += UpdateSlotCount;
					text_Count.text = bottle.ownCapacity.ToString();
					Debug.Log($"물병의 양 {bottle.ownCapacity}");
					go_CountImage.SetActive(true);
					//UpdateSlotCount();
				}
				else
				{
					go_CountImage.SetActive(true);
					text_Count.text = this.ItemCount.ToString();
				}
			}
			else													  //장비 타입 아이템 ->  갯수 UI Off + 내구도 표시UI On
			{
				Equip_Item equipItem = (Equip_Item)this.item;
				this.itemDurable = equipItem.durable;
                go_CountImage.SetActive(true);
				text_Count.text = $"{equipItem.durable}%";
				equipItem.OnUse += UpdateSlotCount;
                //go_CountImage.SetActive(false);
            }
			// 4.슬롯의 아이템 이미지 투명도 1로 변경.
			SetColor(1);
		}


        // Method : **** 슬롯의 아이템 갯수를 기존에 추가. ****
        public void SetSlotCount(int _count)
		{
            ItemCount += _count;
			text_Count.text = ItemCount.ToString();

			if (ItemCount <= 0)
			{
				ClearSlot();
			}
		}
		// Method : **** 슬롯의 아이템 갯수 UI를 현재 갯수에 맞게 업데이트 ****
		public void UpdateSlotCount()
		{
			if (item is Bottle) //물병 아이템이면
			{
				Bottle bottle = (Bottle)item;
				text_Count.text = bottle.ownCapacity.ToString();

			}
			else if(item is Equip_Item) //내구도가 있는 장비 아이템이면
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


        // Method :  **** 슬롯 초기화 ****
        public void ClearSlot()
		{
			if(item is Bottle)
			{
				Bottle bottle = (Bottle)item;
				bottle.OnUseBottle -= UpdateSlotCount;
			}
			item = null;
            ItemCount = 0;
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

		// Method : 대상 슬롯 위에서 드래그 앤 드롭 이벤트 시 그 슬롯에서 호출 ****
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
        // Method : 슬롯 간의 교환 ****
        void ChangeSlot()
        {
            // 1. OnDrop 이벤트가 호출된 슬롯의 데이터들을 미리 복사해둔다.
            Item tempItem = item;
            int tempItemCount = ItemCount;

            // 2.기존의 슬롯에 있던 아이템의 개수를 0으로 변경
            this.ItemCount = 0;

            // 3.  OnDrop 이벤트가 호출된 슬롯에 드래그 중인 아이템을 추가.
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.ItemCount);
			
			// 4. 기존 슬롯의 아이템의 존재 유무에 따라 기존 슬롯을 업데이트한다.
            if (tempItem != null)
            {					  
                DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);

				if(tempItem is Bottle) //4.1 만약 물병 아이템이었다면, 기존의 이벤트를 제거한다.
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

			// 드래그중인 이미지 리턴
			if (BackPackController.inventory_Activated)
			{
				DragSlot.instance.SetColor(0);
				DragSlot.instance.dragSlot = null;

			}
		}
        // Method : 퀵슬롯에서 슬롯 선택하기 ****
        public void SelectSlot_QuickSlot()
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
        // Method : 인벤토리에서 슬롯 선택하기 ****
        void SelectSlot_Inventory()
		{
            // 1. 퀵슬롯의 선택된 슬롯의 선택 이미지를 해제시켜 헷갈리지 않게 한다.
            if (SelectedSlot_QuickSlot.instance.SelectedSlot != null)
            {
                SelectedSlot_QuickSlot.instance.SelectedSlot.SetColorBG(255);
            }
			// 2. 선택된 슬롯은 배경 이미지의 Red 값을 바꿔서 선택된 것을 확인시킨다.
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
