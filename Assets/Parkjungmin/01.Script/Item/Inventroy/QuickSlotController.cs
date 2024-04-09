using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jungmin
{
	public class QuickSlotController : MonoBehaviour
	{
		/*
		 * 인벤토리가 켜져 있을 경우는 슬롯 간 상호 작용 가능 
		 * 
		 * 인벤토리가 꺼져 있을 경우는 퀵슬롯 안의 무기를 선택하여 사용하기만 가능.
		 * 
		 * 
		 */
		[SerializeField] Slot[] slots;
		[SerializeField] GameObject Slot_parent;
		[SerializeField] GameObject quickSlot_Base;
		//[SerializeField] GameObject default_Slot;
 
        private void Start()
		{
            
            slots = Slot_parent.GetComponentsInChildren<Slot>();
            //SelectedSlot_QuickSlot.instance.SelectedSlot = default_Slot.GetComponent<Slot>();
        }

	}
}
     