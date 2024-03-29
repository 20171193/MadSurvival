using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
	public DragSlot instance;

	[SerializeField] Slot TargetSlot;

	Image image;

	private void Start()
	{
		instance = this;
	}

	void DragSetImage()
	{
		if(TargetSlot.item != null )
		{
			//image = TargetSlot.item.icon;
		}
	}


}
