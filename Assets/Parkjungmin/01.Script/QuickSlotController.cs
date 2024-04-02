using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManage : MonoBehaviour
{
	[SerializeField] QuickSlot [] quickSlots; 
	[SerializeField] Transform tf_parent; //Äü½º·ÔÀÇ ºÎ¸ð ¿ÀºêÁ§Æ®
	public GameObject SlotSet;

	int selectSlot; //¼±ÅÃµÈ Äü½½·ÔÀÇ ÀÎµ¦½º ( 0~7)
	[SerializeField] GameObject go_SelectImage;

	[SerializeField]
	WeaponManager theWeaponManager;

	


	private void Start()
	{
		quickSlots = SlotSet.GetComponentsInChildren<QuickSlot>();
		selectSlot = 0;
	}
	private void Update()
	{

	}
	private void TryInputNumber()
	{
		if ( Input.GetKeyDown(KeyCode.Alpha1) )
			ChangeSlot(0);
		else if ( Input.GetKeyDown(KeyCode.Alpha2) )
			ChangeSlot(1);
		else if ( Input.GetKeyDown(KeyCode.Alpha3) )
			ChangeSlot(2);
		else if ( Input.GetKeyDown(KeyCode.Alpha4) )
			ChangeSlot(3);
		else if ( Input.GetKeyDown(KeyCode.Alpha5) )
			ChangeSlot(4);
		else if ( Input.GetKeyDown(KeyCode.Alpha6) )
			ChangeSlot(5);
	}
	void ChangeSlot(int _num )
	{
		SelectedSlot(_num);
		
	}
	void SelectedSlot(int _num )
	{
		//¼±ÅÃµÈ ½½·Ô È°¼ºÈ­.
		selectSlot = _num;

		go_SelectImage.transform.position = quickSlots [selectSlot].transform.position;

	}
	void Execute()
	{
		if( quickSlots[selectSlot].item != null )
		{
			if(quickSlots[selectSlot].item.itemtype == Item.ItemType.Equipment )
			{
			} 
		}
	}

}
