using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jungmin
{
	public class QuickSlotController : MonoBehaviour
	{
		[SerializeField] static QuickSlotController instance;
		/*
		 * �κ��丮�� ���� ���� ���� ���� �� ��ȣ �ۿ� ���� 
		 * 
		 * �κ��丮�� ���� ���� ���� ������ ���� ���⸦ �����Ͽ� ����ϱ⸸ ����.
		 * 
		 * 
		 *
		 */

		[SerializeField] public Slot[] slots;
		[SerializeField] public GameObject Slot_parent;
		[SerializeField] public GameObject quickSlot_Base;
		//[SerializeField] GameObject default_Slot;
 
        private void Start()
		{
			instance = this;
            slots = Slot_parent.GetComponentsInChildren<Slot>();
            //SelectedSlot_QuickSlot.instance.SelectedSlot = default_Slot.GetComponent<Slot>();
        }

	}
}
     