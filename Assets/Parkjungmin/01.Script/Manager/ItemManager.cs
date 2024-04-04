using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
namespace jungmin
{
    public enum ItemType
    {
        Crafting,
        Ingredient
    }
}
public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<string, Item> craftingItemDic = new Dictionary<string, Item>();
    public Dictionary<string, Item> ingredientItemDic = new Dictionary<string, Item>();

    private void OnEnable()
    {
        LoadItem(ItemType.Ingredient);
        LoadItem(ItemType.Crafting);
    }    
    private void LoadItem(ItemType type)
    {
        switch(type)
        {
            case ItemType.Crafting:
                Item[] items = Resources.LoadAll<Item>("CraftingItem");
                break;
            case ItemType.Ingredient:
                break;
        }
    }

    public void CraftItem()
        //아이템의 만드는 기능,인벤토리 창의 Craft 버튼 이벤트가 여기로 연결된다.
    {
        /* 0. 설계도 슬롯의 특정 슬롯 선택
         * 1. Craft 버튼 누름
         * 2. 슬롯 배열을 확인하면서, 만들려는 아이템의 재료들을 전부 가지고 있는지 확인
         * 3. 없었다면, 실패를 띄우거나 그냥 무시되도록
         * 4. 성공한다면 해당 아이템이 AcquireItem 함수를 통해 인벤토리에 추가됨
         */
    }

    //public void ShowRecipeInfo()
    //{
    //    if (SelectedSlot_Recipe.instance.slot != null)
    //    {
    //        Recipe_Info.text =
    //        SelectedSlot_Recipe.instance.slot.recipe


    //    }
    //}
}
