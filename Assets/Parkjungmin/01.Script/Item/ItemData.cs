using JetBrains.Annotations;
using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemBase",menuName ="ItemBase/BaseItem")]
[Serializable]
public class ItemData : ScriptableObject
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

    [Header("������ ��ųʸ��� Resource �������� ��ũ���ͺ� ������Ʈ�� �а�")]
    [Header("�Ʒ��� Recipe ����� CSV ���Ͽ��� ����")]
	[SerializeField] public Recipe recipe; //����� ������
}
