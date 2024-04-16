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
            Equipment, // 장비(갯수 카운트X)
            Used, // 소비 아이템
            Ingredient, //재료
            Structure // 빌딩.
        }

        [Header("필수*아이템의 이름")]
        [SerializeField] public string itemName;
        [Header("필수*아이템의 이미지")]
        [SerializeField] public Sprite itemImage;
        [Header("필수*아이템의 타입")]
        [SerializeField] public ItemType itemtype;
        [Header("선택*툴팁에서 보여줄 아이템의 설명")]
        [SerializeField] public string itemInfo;

        [Header("****새 아이템 조합법 추가방법****")]
        [Header("1.ParkJungMin/05.Data/Resources/RecipeCSV 열기")]
        [Header("2.새로운 아이템의 이름과 조합식을 적은 뒤, 저장.")]
        [Header("****새 아이템 추가방법****")]
        [Header("1. 마우스 오른쪽 버튼 + Create/ItemData/ItemdData_SO")]
        [Header("2  필수 항목 작성 후, ParkJungMin/04.ObjectData/Resources/ScriptableObject에 저장")]
        [Header("3. 빈 오브젝트 생성 후, ParkJungMin/01.Scripts/Item에서 아이템의 종류에 맞는 C#파일을 오브젝트에 삽입.")]
        [Header("4. C# 파일을 삽입하면 ItemData 필드가 보일텐데, 그곳에 2번에서 만든 ScriptableObject 파일 끌어다 삽입.")]
        [Header("5. 프리팹으로 만들어 Parkjungmin/04.ObjectData/Resources의 CraftingItem 혹은 IngredientItem 폴더에 저장.")]
        [Header("주의 : ScriptableObject의 필수 항목의 작성 및 아이템의 이름,아이템의 조합식에서의 이름이 틀려서는 안된다.")]
        public Recipe recipe; //재료의 레시피
    }
}