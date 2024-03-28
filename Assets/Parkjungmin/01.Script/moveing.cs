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




		private void Update()
		{
			transform.Translate(dir * movespeed * Time.deltaTime, Space.World);
		}



		void OnMove(InputValue value)
		{
			Vector2 dir_ = value.Get<Vector2>();

			dir = new Vector3(dir_.x, 0, dir_.y);
		}

		void OnBackPack(InputValue value)
		{

		}


	}
}
