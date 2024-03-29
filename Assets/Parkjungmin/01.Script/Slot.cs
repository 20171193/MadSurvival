using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IPointerDownHandler,IDragHandler,IDropHandler
{

	[SerializeField] public Item item;
	int itemCount;
	Vector3 firstPos;
	
	private void Update()
	{
		if ( item != null )
		{
			transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
		}  //�� ���Կ� �������� �ִٸ� ������ ��������Ʈ�� ǥ��
		else if ( item == null )
		{
			transform.GetChild(0).GetComponent<Image>().sprite = null;
		} // ���� ���� ����α�
	}


	public void OnDrop(PointerEventData data)
	{

	}
	public void OnPointerDown(PointerEventData data )
	{
		if(item != null )
		{
			// transform.GetChild(1).GetComponent<RectTransform>()
			transform.GetChild(1).position = Input.mousePosition;
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
