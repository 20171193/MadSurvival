using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

	[SerializeField] public string name;
	[SerializeField] public Sprite icon;
	[SerializeField] itemData itemdata;


	private void OnTriggerEnter( Collider other )
	{
		if (other.gameObject.GetComponent<moveing>())
		{
			other.gameObject.GetComponent<moveing>().Inventory.GetItem(this);
			gameObject.SetActive(false);
		}

	}
}
