using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
	public static DragSlot instance;

	[SerializeField] public  Slot TargetSlot;

	public Image image;

	private void Start()
	{
		instance = this;
	}

	void DragSetImage()
	{
		if(TargetSlot.item != null )
		{
			image = TargetSlot.transform.GetChild(1).GetComponent<Image>();
			SetColor(1);
		}
	}
	void SetColor(float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}



}
