using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jungmin
{
	public class WeaponManager : MonoBehaviour
	{
		static bool isChangeWeapon;
		[SerializeField] public Transform currentWeapon;
		[SerializeField] Dictionary<string, Item> ItemDictionary = new Dictionary<string, Item>();
		[SerializeField] string curItemType;

		[SerializeField] BuildController buildController;

		private void Start()
		{

		}

		public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
		{
			isChangeWeapon = true;

			yield return new WaitForSeconds(1f);

			isChangeWeapon = false;
		}
	}
}
