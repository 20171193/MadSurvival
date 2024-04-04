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
		Equipment, // ���(���� ī��ƮX)
		Used, // �Һ� ������
		ingredient, //���
	}
	[SerializeField] public string itemName;
	[SerializeField] public Sprite itemImage;
	[SerializeField] public ItemType itemtype;
	//[SerializeField] GameObject DropPrefab;

}
