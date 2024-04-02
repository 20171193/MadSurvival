using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItem",menuName ="NewItem/item")]
public class Item : ScriptableObject
{
	public enum ItemType
	{
		Equipment, // 장비
		Countable, //소모 아이템
		ingredient, //재료
	}
	[SerializeField] public string name;
	[SerializeField] public Sprite icon;
	[SerializeField] public ItemType itemtype;
	[SerializeField] GameObject DropPrefab;
	public int Count;

}
