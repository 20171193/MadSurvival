//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.EventSystems;
//using UnityEngine;
//using UnityEngine.UI;

//public class BackPack: MonoBehaviour ,IPointerDownHandler,IBeginDragHandler, IPointerUpHandler
//{
//	//�巡�� �� ��� , ���,
//	//������ ���ý�(��ġ) : ����, ������
//	//���� 3X10
//	//ũ������ 2*10 + alpha;; ��ư������ ���� ������
//	//���â 1;
//	//������ 20 + alpha
//	/*�ѤѤѤ�
//	 * �巡�� ������ ������ ���� ����
//	 * ������ ȹ�� �� 
//	 * 
//	 * 
//	 * �ѤѤѤѤ�
//	 * ���� �� -> Time.timeScale = 0f;
//	 * <<Craft>>
//	 * ������: ����,���,���,���๰,
//	 */
//	List<List<Slot>> backpack = new List<List<Slot>>();
//	List<RaycastResult> list = new List<RaycastResult>();
//	Slot CurArmor;
//	GraphicRaycaster Isclickable;

//	public Sprite beingDragIcon;
//	Vector3 startPostion;

//	[SerializeField] Transform onDragParent;
//	[HideInInspector] public Transform startParent;

//	Vector3 DragStartPos;
//	Vector3 DragStayPos;
//	PointerEventData MousePos;

//	private void Update()
//	{
//		MousePos.position = Input.mousePosition;


//		OnPointerDown();
//		OnPointerUp();
//		OnPointerDrag();

		
//	}

//	public void OnPointerUp( )
//	{

//	}

//	public void OnPointerDrag()
//	{
//		//beingDragIcon = gameObject;

//	}
//	public void OnPointerDown()
//	{ 
//	}
//	public void OnPointerDown()
//	{

//	}


//	void Swap()
//	{

//	}
//	void Drop()
//	{

//	}
//	void Craft()
//	{

//	}




//}

