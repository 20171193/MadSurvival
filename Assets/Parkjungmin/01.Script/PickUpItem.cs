using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PickUpItem : MonoBehaviour
{
	public Item item;

	[SerializeField] BackPackController backpack;

	private void OnTriggerEnter( Collider other )
	{
		if(other.gameObject.tag == "Player" )
		{
			backpack.AcquireItem(item);
			gameObject.SetActive(false);
		}
	}
}
