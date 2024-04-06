using jungmin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler,IPointerClickHandler
{
	Vector3 originPos;
	public ItemData item;
	public int itemCount;
	[SerializeField] public Image itemImage; //슬롯 위에 보여질 아이템 이미지.
	[SerializeField] TMP_Text text_Count;
	[SerializeField] GameObject go_CountImage;
	private void Start()
	{
		originPos = transform.position;
	}
	public void AddItem( ItemData _item, int _count = 1 ) //슬롯 상의 아이템 UI 업데이트
	{
		item = _item;
		itemCount = _count;

        itemImage.sprite = ItemManager.Instance.craftingItemDic[_item.itemName].itemImage;

        if (item.itemtype != ItemData.ItemType.Equipment) //체력,소비 아이템이면,
		{
			go_CountImage.SetActive(true);
			text_Count.text = itemCount.ToString(); 
		}
		else
		{
			text_Count.text = "0";
			go_CountImage.SetActive(false);
		}

		SetColor(1);
	}
	public void SetSlotCount(int _count )
	{
		itemCount += _count;
		text_Count.text = itemCount.ToString();

		if ( itemCount <= 0 )
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
	void SetColor(float alpha ) //슬롯의 아이템 이미지의 색깔을 변경
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

    public void OnBeginDrag( PointerEventData eventData )
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
	public void OnDrag( PointerEventData eventData )
	{
		// 이미지 드래그
		if(BackPackController.inventory_Activated)
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

    public void OnEndDrag( PointerEventData eventData )
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
		ItemData tempItem = item;
		int tempItemCount = itemCount;

		AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
		if(tempItem != null)
		{
			DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
		}
		else
		{

			DragSlot.instance.dragSlot.ClearSlot();
		}
	}
	void SelectSlot()
	{
        //인벤토리가 꺼져있을 때만 반응하기에, 인벤토리와 무관.
        // 퀵슬롯에서 슬롯 선택하기
        if (!BackPackController.inventory_Activated)
        {
			if (SelectedSlot.instance.slot != null) //이전에 셀렉된 슬롯이 있었다면,
			{
				SelectedSlot.instance.slot.SetColorBG(255);
				Debug.Log("SelectSlot 교체 실행");
				SelectedSlot.instance.slot = this;
				SetColorBG(0);
			}
			else
			{
                SelectedSlot.instance.slot = this;
                SetColorBG(0);
            }
        }

    }
	void ShowToolTip()
    {// 인벤토리가 켜져있을 때만 툴팁
        if (BackPackController.inventory_Activated)
		{
			//Debug.Log("Show Tool Tip");
		}

	}
    public void OnPointerClick(PointerEventData eventData)
    {
		/* 인벤토리가 켜져있을 땐 아이템 설명 혹은 툴팁
		 * 인벤토리가 꺼져있을 땐 퀵슬롯의 슬롯 선택
		 * 
		 */
		//Debug.Log("OnPointerClick event");
		if (BackPackController.inventory_Activated)
		{
            ShowToolTip();
            
		}
		else
		{
            SelectSlot();
        }
    }
}
