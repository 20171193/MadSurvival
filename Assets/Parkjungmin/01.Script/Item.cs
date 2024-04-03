using jungmin;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItem",menuName ="NewItem/item")]
public class Item : ScriptableObject
{
	public enum ItemType
	{
		Equipment, // ���
		Countable, //�Ҹ� ������
		ingredient, //���
	}
	[SerializeField] public string itemName;
	[SerializeField] public Sprite itemImage;
	[SerializeField] public ItemType itemtype;
	//[SerializeField] GameObject DropPrefab;

}
