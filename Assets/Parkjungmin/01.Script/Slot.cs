using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler
{
	Vector3 originPos;
	public Item item;
	public int itemCount;
	public Image itemimage;
	[SerializeField] TMP_Text text_Count;
	[SerializeField] GameObject go_CountImage;
	private void Start()
	{
		originPos = transform.position;
	}
	public void AddItem( Item _item, int _count = 1 )
	{
		item = _item;
		itemCount = _count;
		itemimage.sprite = _item.itemImage; //Áß¿ä

		if(item.itemtype != Item.ItemType.Equipment)
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
	void ClearSlot()
	{
		item = null;
		itemCount = 0;
		itemimage.sprite = null;
		SetColor(0);
		text_Count.text = "0";
		go_CountImage.SetActive(false);

	}
	void SetColor(float alpha )
	{
		Color color = itemimage.color;
		color.a = alpha;
		itemimage.color = color;
	}

	public void OnBeginDrag( PointerEventData eventData )
	{
		Debug.Log("Call beginDrag");
		if(item != null)
		{
			DragSlot.instance.transform.position = eventData.position;
			DragSlot.instance.dragSlot = this;
			DragSlot.instance.DragSetImage(itemimage);
		}
	}
	public void OnDrag( PointerEventData eventData )
	{
		Debug.Log("Call Drag");
		if (item != null )
		{
			DragSlot.instance.transform.position = eventData.position;
		}
	}
	public void OnEndDrag( PointerEventData eventData )
	{
		DragSlot.instance.SetColor(0);
		DragSlot.instance.dragSlot = null;
	}
	public void OnDrop( PointerEventData eventData )
	{
		Debug.Log("OnDrop");
		if ( DragSlot.instance.dragSlot != null )
		{
			Debug.Log("Changing Slot");
			ChangeSlot();
		}
		else
		{
			Debug.Log("DragSlot.instance.dragSlot is null");
		}

	}
	void ChangeSlot()
	{
		Item tempItem = item;
		int tempItemCount = itemCount;

		AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
		if(tempItem != null)
		{
			DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
		}
		else
		{
			Debug.Log("2");
			DragSlot.instance.dragSlot.ClearSlot();
		}
	}
}
