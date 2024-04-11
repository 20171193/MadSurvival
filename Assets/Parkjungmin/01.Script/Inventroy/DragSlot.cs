using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jungmin
{
	public class DragSlot : MonoBehaviour
	{
		public static DragSlot instance;
		public Slot dragSlot;

		[SerializeField] public Image imageItem;
		private void Start()
		{
			instance = this;
		}

		// Method : �巡�� ���� �������� �̹��� ������ ****
		public void DragSetImage(Image itemimage_)
		{
			imageItem.sprite = itemimage_.sprite;
			SetColor(1);

		}
		// Method : �巡�� ���� ������ �̹����� ���� ���� ****
		public void SetColor(float alpha)
		{
			Color color = imageItem.color;
			color.a = alpha;
			imageItem.color = color;
		}
		private void OnDrawGizmos()
		{

		}
	}
}