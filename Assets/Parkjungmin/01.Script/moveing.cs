using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace jungmin
{


	public class moveing : MonoBehaviour
	{
		Vector3 dir;
		[SerializeField] float movespeed;
		Vector2 ancorPos;

		[SerializeField] GameObject backPackOb;

		private void Awake()
		{
		}

		private void Update()
		{
			transform.Translate(dir * movespeed * Time.deltaTime, Space.World);
		}

		private void OnEnable()
		{
			backPackOb.SetActive(false);
		}


		void OnMove(InputValue value)
		{
			Vector2 dir_ = value.Get<Vector2>();

			dir = new Vector3(dir_.x, 0, dir_.y);
		}

		void OnBackpack(InputValue value ) //���� ����
		{

		}


	}
}
