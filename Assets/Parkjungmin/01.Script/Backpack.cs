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


	/*
	 * 
	 * 
	 * 
	 */
	[SerializeField] public Slot[] backpack;
	[SerializeField] public GameObject SlotSet;
	List<RaycastResult> list = new List<RaycastResult>();
	Slot CurArmor;
	Vector2 OpenPos;
	Vector2 hidePos;
	[SerializeField] public bool Ishide;

	private void Awake()
	{
		backpack = SlotSet.GetComponentsInChildren<Slot>();
		OpenPos = new Vector2(0, -48.1f);
		hidePos = new Vector2(-1600, -48.1f);
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
			if (backpack[x].item == null )
			{
				backpack [x].item = z;
				Debug.Log($"{x}�� ���Կ� ����Ǿ����ϴ�.");
				return;
			}
		}
	}
	public void Close()
	{
		//transform.parent.GetComponent<RectTransform>().anchoredPosition = hidePos;
		Ishide = false;
	}
	public void Open()
	{
		//transform.parent.GetComponent<RectTransform>().anchoredPosition = OpenPos;
		Ishide = true;
	}
	void Craft()
	{ 
	}

}

