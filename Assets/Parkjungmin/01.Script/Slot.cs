using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IPointerDownHandler,IDragHandler,IDropHandler,IPointerUpHandler
{

	[SerializeField] public Item item;
	int itemCount;
	public Vector2 firstPos;
	public Vector2 NewSlotPos;
	Sprite DragSprite;
	public bool IsDraggSprite; //��������Ʈ�� �巡�� ������.
	public bool IsSlotAnchor; //�巡�� ���� �������� ������ �ݶ��̴� �ȿ� ���Դ��� ����.

	private void Update()
	{
		if ( item != null )
		{
			DragSprite = transform.GetChild(1).GetComponent<Image>().sprite;
			transform.GetChild(1).gameObject.SetActive(true);
			transform.GetChild(1).GetComponent<Image>().sprite = item.icon;
		}  //�� ���Կ� �������� �ִٸ� ������ ��������Ʈ�� ǥ��
		else if ( item == null )
		{
			transform.GetChild(1).gameObject.SetActive(false);
		} // ���� ���� ����α�
	}


	public void OnDrop(PointerEventData data)
	{
		if(IsDraggSprite && IsSlotAnchor )
		{

		}
	}
	public void OnPointerDown(PointerEventData data )
	{
		if(item != null )
		{
			firstPos = transform.GetChild(1).position;
			IsDraggSprite = true;
		}
	}
	public void OnPointerUp(PointerEventData data)
	{
		if(IsDraggSprite && (IsSlotAnchor == false) )
		{
			transform.GetChild(1).position = firstPos;
			IsDraggSprite = false;
		}

		if ( IsDraggSprite && IsSlotAnchor)
		{
			transform.GetChild(1).position = NewSlotPos;
			IsSlotAnchor = false;
			IsDraggSprite = false;
		}
	}
	public void OnDrag(PointerEventData data )
	{
		if(item != null )
		{
			transform.GetChild(1).position = Input.mousePosition;
		}
	}


}
