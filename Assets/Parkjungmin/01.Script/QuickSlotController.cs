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
		private void Start()
		{
			slots = Slot_parent.GetComponentsInChildren<Slot>();
		}
		void Execute()
		{
			//���õ� ���Կ� �ִ� �������� ����ϱ�.
		}

	}
}
