using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BackPack : MonoBehaviour
{
	//�巡�� �� ��� , ���,
	//������ ���ý�(��ġ) : ����, ������
	//���� 3X10
	//ũ������ 2*10 + alpha;; ��ư������ ���� ������
	//���â 1;
	//������ 20 + alpha
	/*�ѤѤѤ�
	 * �巡�� ������ ������ ���� ����
	 * ������ ȹ�� �� 
	 * 
	 * 
	 * �ѤѤѤѤ�
	 * ���� �� -> Time.timeScale = 0f;
	 * <<Craft>>
	 * ������: ����,���,���,���๰,
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
			Debug.Log("�Ҵ� �Ϸ�");
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
				Debug.Log($"{x}�� ���Կ� ����Ǿ����ϴ�.");
				return;
			}
		}
	}
	void Craft()
	{ 
	}

}

