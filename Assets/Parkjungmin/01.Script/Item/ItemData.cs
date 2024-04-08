using JetBrains.Annotations;
using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


namespace jungmin
{
	[CreateAssetMenu(fileName ="ItemData",menuName ="ItemData/ItemdData_Base")]
	[Serializable]
	public class ItemData : ScriptableObject
	{
		public enum ItemType
		{
			Equipment, // ���(���� ī��ƮX)
			Used, // �Һ� ������
			Ingredient, //���
            Structure // ����.
        }

		[SerializeField] public string itemName;
		[SerializeField] public Sprite itemImage;
		[SerializeField] public ItemType itemtype;

		[Header("������ ��ųʸ��� Resource �������� ��ũ���ͺ� ������Ʈ�� �а�")]
		[Header("�Ʒ��� Recipe ����� CSV ���Ͽ��� ����")]
		[SerializeField] public Recipe recipe; //����� ������
	}
}
