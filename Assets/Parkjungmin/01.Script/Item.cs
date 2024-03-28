using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
	[SerializeField] public Sprite icon;
	[SerializeField] itemData itemdata;
	


	private void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.GetComponent<BackPack>() )
		{
			Destroy(gameObject);

			
		}

	}
}
