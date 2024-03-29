using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BackPack : MonoBehaviour
{
	//드래그 앤 드롭 , 취소,
	//아이템 선택시(터치) : 장착, 버리기
	//백팩 3X10
	//크래프팅 2*10 + alpha;; 버튼식으로 다음 스왑핑
	//장비창 1;
	//제조법 20 + alpha
	/*ㅡㅡㅡㅡ
	 * 드래그 조건은 누르고 있을 동안
	 * 아이템 획득 시 
	 * 
	 * 
	 * ㅡㅡㅡㅡㅡ
	 * 백팩 온 -> Time.timeScale = 0f;
	 * <<Craft>>
	 * 아이템: 음식,장비,재료,건축물,
	 */
	public Slot[] backpack;
	List<RaycastResult> list = new List<RaycastResult>();
	Slot CurArmor;
	GraphicRaycaster Isclickable;
	Vector3 startPostion;
	[HideInInspector] public Transform startParent;

	Vector3 DragStartPos;
	Vector3 DragStayPos;
	PointerEventData MousePos;

	private void Awake()
	{
		backpack = transform.GetChild(1).gameObject.GetComponentsInChildren<Slot>(); 
		if(backpack != null )
		{
			Debug.Log("할당 완료");
		}
	}

	void Swap()
	{

	}
	void Drop()
	{

	}
	public void GetItem( Item z )
	{
		for(int x=0;x<backpack.Length;x++)
		{
			if(backpack[x].item == null )
			{
				backpack [x].item = z;
				Debug.Log($"{x}번 슬롯에 저장되었습니다.");
				return;
			}
		}
	}
	void Craft()
	{ 
	}

}

