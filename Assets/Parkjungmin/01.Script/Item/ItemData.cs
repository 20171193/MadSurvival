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
		Equipment, // 장비(갯수 카운트X)
		Used, // 소비 아이템
		ingredient, //재료
	}

	[SerializeField] public string itemName;
	[SerializeField] public Sprite itemImage;
	[SerializeField] public ItemType itemtype;

    [Header("아이템 딕셔너리는 Resource 폴더에서 스크럽터블 오브젝트를 읽고")]
    [Header("아래의 Recipe 목록은 CSV 파일에서 읽음")]
	[SerializeField] public Recipe recipe; //재료의 레시피
}
