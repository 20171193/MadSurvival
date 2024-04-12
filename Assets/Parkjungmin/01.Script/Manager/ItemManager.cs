using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Linq;
using Unity.VisualScripting;
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
    bool ready_Craft;
    private void Start()
    {
        LoadItem(ItemType.Ingredient);
        LoadItem(ItemType.Crafting);
    }

    // Method : 아이템의 타입에 따라 구분해 각각의 Dic에 추가 ****
    private void LoadItem(ItemType type)
    {
        // 1.Resource의 CraftingItem,IngredientItem 폴더에서 모든 Item 속성들 갖고 있는 프리팹들을 수집한다.
        switch (type)
        {
            case ItemType.Crafting: // 2. 크래프팅 속성이라면 크래프팅 Dic에 데이터 추가
                Item[] craft_items = Resources.LoadAll<Item>("CraftingItem"); 
                foreach(Item item in craft_items)
                {
                    string name = item.itemdata.itemName;
                    //Debug.Log($"Craft: {name}");
                    if (craftingItemDic.ContainsKey(name)) { continue; }
                    craftingItemDic.Add(name, item);

                }
                break;
            case ItemType.Ingredient: // 2. 재료 속성이라면 크래프팅 Dic에 데이터 추가
                Item[] igd_items = Resources.LoadAll<Item>("IngredientItem");
                foreach (Item item in igd_items)
                { 
                    string name = item.itemdata.itemName;
                    //Debug.Log($"IGD : {name}");
                    if (ingredientItemDic.ContainsKey(name)) { continue; }
                    ingredientItemDic.Add(name, item);

                }
                break;
        }
    }


    // Method : 크래프팅 전에 인벤토리에 충분한 재료 소지 여부 확인 ****
    void SearchForCraft()
    {
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        ready_Craft = false;
        for (int x = 0; x < BackPackController.instance.slots.Length; x++) // 백팩 슬롯의 갯수 만큼
        {
            
            if(BackPackController.instance.slots[x].item == null)
            {
                continue;
            }
            if (BackPackController.instance.slots[x].item != null)
            {
                if (BackPackController.instance.slots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name)
                {
                    if (BackPackController.instance.slots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                    {

                        IGD_1_Check = true;
                        if(SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                        {
                            break;
                        }
                        else if(SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == null && SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null )
                        { //조합식 항이 1개밖에 없을 경우.
                            ready_Craft = true;
                        }
                        break;
                    }
                    else
                    {
                        IGD_1_Check = false;
                        break;
                    }
                }
            }
            
        }

        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null && IGD_1_Check)
        {
            for (int x = 0; x < BackPackController.instance.slots.Length; x++) // 백팩 슬롯의 갯수 만큼
            {

                Debug.Log($"두번째 재료 탐색");
                if (BackPackController.instance.slots[x].item == null)
                {
                    Debug.Log("이 슬롯엔 그 아이템이 없습니다.");
                    continue;
                }

                if (BackPackController.instance.slots[x].item != null)
                {
                    if (BackPackController.instance.slots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name)
                    {
                        if (BackPackController.instance.slots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
                        {
                            Debug.Log("재료 아이템이 충분합니다.");
                            IGD_2_Check = true;
                            if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                            { 
                                break;
                            }
                            else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                            { //조합식 항이 2개뿐일 경우.
                                ready_Craft = true;
                            }
                            break;
                        }
                        else
                        {
                            IGD_2_Check = false;
                            break;
                        }
                    }
                }

            }
        }
        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null && IGD_1_Check && IGD_2_Check)
        {
            for (int x = 0; x < BackPackController.instance.slots.Length; x++) // 백팩 슬롯의 갯수 만큼
            {

                Debug.Log($"세번째 재료 탐색");
                if (BackPackController.instance.slots[x].item == null)
                {
                    Debug.Log("이 슬롯엔 그 아이템이 없습니다.");
                    continue;
                }
                if (BackPackController.instance.slots[x].item != null)
                {
                    if (BackPackController.instance.slots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name)
                    {
                        if (BackPackController.instance.slots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                        {

                            ready_Craft = true;
                            break;
                        }
                    }
                }

            }
        }

        return;
    }

    // Method : 크래프팅 기능 ****
    public void CraftItem()
    {

        if (SelectedSlot_Recipe.instance.slot != null) 
        {
            SearchForCraft(); // 1. 먼저 재료가 인벤토리 내에 충분히 있는지 확인한다.

            if (ready_Craft)
            {
                BackPackController.instance.AcquireItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe_name]);
                Debug.Log("크래프팅 생성");

                if(craftingItemDic.ContainsKey(SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name))
                    BackPackController.instance.LoseItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);
                else
                    BackPackController.instance.LoseItem(ingredientItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);

                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                {
                    if (craftingItemDic.ContainsKey(SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name))
                        BackPackController.instance.LoseItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                    else
                        BackPackController.instance.LoseItem(ingredientItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                }
                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                {
                    if (craftingItemDic.ContainsKey(SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name))
                        BackPackController.instance.LoseItem(craftingItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                    else
                        BackPackController.instance.LoseItem(ingredientItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                }

                //아이템을 생성한 기능

                //재료로 소모한 아이템의 개수 줄이는 기능 추가
            }
            else
            {
                Debug.Log("재료 부족");
            }

        }
    }
}
