using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace jungmin
{


	public class moveing : MonoBehaviour
	{
		Vector3 dir;
		[SerializeField] float movespeed;
		[SerializeField] public BackPack Inventory;
		Vector2 ancorPos;

		private void Awake()
		{
		}

		private void Update()
		{
			transform.Translate(dir * movespeed * Time.deltaTime, Space.World);
		}



		void OnMove(InputValue value)
		{
			Vector2 dir_ = value.Get<Vector2>();

			dir = new Vector3(dir_.x, 0, dir_.y);
		}

		void OnBackpack(InputValue value ) //¹éÆÑ ¿ÀÇÂ
		{
			if ( Inventory.Ishide == false )
			{
				Inventory.Open();
			}
			else if( Inventory.Ishide == true)
			{
				Inventory.Close();
			}

		}


	}
}
