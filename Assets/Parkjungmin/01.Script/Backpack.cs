//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.EventSystems;
//using UnityEngine;
//using UnityEngine.UI;

//public class BackPack: MonoBehaviour ,IPointerDownHandler,IBeginDragHandler, IPointerUpHandler
//{
//	//드래그 앤 드롭 , 취소,
//	//아이템 선택시(터치) : 장착, 버리기
//	//백팩 3X10
//	//크래프팅 2*10 + alpha;; 버튼식으로 다음 스왑핑
//	//장비창 1;
//	//제조법 20 + alpha
//	/*ㅡㅡㅡㅡ
//	 * 드래그 조건은 누르고 있을 동안
//	 * 아이템 획득 시 
//	 * 
//	 * 
//	 * ㅡㅡㅡㅡㅡ
//	 * 백팩 온 -> Time.timeScale = 0f;
//	 * <<Craft>>
//	 * 아이템: 음식,장비,재료,건축물,
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

