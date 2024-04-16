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
    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/ItemdData_SO")]
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

        [Header("�ʼ�*�������� �̸�")]
        [SerializeField] public string itemName;
        [Header("�ʼ�*�������� �̹���")]
        [SerializeField] public Sprite itemImage;
        [Header("�ʼ�*�������� Ÿ��")]
        [SerializeField] public ItemType itemtype;
        [Header("����*�������� ������ �������� ����")]
        [SerializeField] public string itemInfo;

        [Header("****�� ������ ���չ� �߰����****")]
        [Header("1.ParkJungMin/05.Data/Resources/RecipeCSV ����")]
        [Header("2.���ο� �������� �̸��� ���ս��� ���� ��, ����.")]
        [Header("****�� ������ �߰����****")]
        [Header("1. ���콺 ������ ��ư + Create/ItemData/ItemdData_SO")]
        [Header("2  �ʼ� �׸� �ۼ� ��, ParkJungMin/04.ObjectData/Resources/ScriptableObject�� ����")]
        [Header("3. �� ������Ʈ ���� ��, ParkJungMin/01.Scripts/Item���� �������� ������ �´� C#������ ������Ʈ�� ����.")]
        [Header("4. C# ������ �����ϸ� ItemData �ʵ尡 �����ٵ�, �װ��� 2������ ���� ScriptableObject ���� ����� ����.")]
        [Header("5. ���������� ����� Parkjungmin/04.ObjectData/Resources�� CraftingItem Ȥ�� IngredientItem ������ ����.")]
        [Header("���� : ScriptableObject�� �ʼ� �׸��� �ۼ� �� �������� �̸�,�������� ���սĿ����� �̸��� Ʋ������ �ȵȴ�.")]
        public Recipe recipe; //����� ������
    }
}