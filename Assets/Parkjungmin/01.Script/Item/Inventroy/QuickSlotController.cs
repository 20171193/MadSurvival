using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jungmin
{
	public class QuickSlotController : MonoBehaviour
	{
		/*
		 * �κ��丮�� ���� ���� ���� ���� �� ��ȣ �ۿ� ���� 
		 * 
		 * �κ��丮�� ���� ���� ���� ������ ���� ���⸦ �����Ͽ� ����ϱ⸸ ����.
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
     