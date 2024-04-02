using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
	public Item item;
	[SerializeField] RectTransform baseRect; //인벤토리 영역 
	[SerializeField] RectTransform quickSlotBaseRect; //퀵슬롯

	private void Start()
	{
	}

}
