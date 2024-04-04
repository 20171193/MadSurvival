using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItem",menuName ="NewItem/item")]
[Serializable]
public class Item : ScriptableObject
{
	public enum ItemType
	{
		Equipment, // 장비(갯수 카운트X)
		Used, // 소비 아이템
		ingredient, //재료
	}
	[SerializeField] public string itemName;
	[SerializeField] public Sprite itemImage;
	[SerializeField] public ItemType itemtype;
	//[SerializeField] GameObject DropPrefab;

}
