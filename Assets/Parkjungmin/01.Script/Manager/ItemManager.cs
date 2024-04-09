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
    private void LoadItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Crafting:
                Item[] craft_items = Resources.LoadAll<Item>("CraftingItem"); //아이템 딕셔너리에 데이터 할당
                foreach(Item item in craft_items) // 05.Scriptable Object에서 검색해서 할당함
                { // 정상적으로 작동하기 위해선, 
                    string name = item.itemdata.itemName;
                    //Debug.Log($"{name}");
                    craftingItemDic.Add(name, item);

                }
                break;
            case ItemType.Ingredient:
                Item[] igd_items = Resources.LoadAll<Item>("IngredientItem"); //아이템 딕셔너리에 데이터 할당
                foreach (Item item in igd_items) // 05.Scriptable Object에서 검색해서 할당함
                { // 정상적으로 작동하기 위해선, 
                    string name = item.itemdata.itemName;
                    //Debug.Log($"{name}");
                    ingredientItemDic.Add(name, item);

                }
                break;
        }
    }
    void SearchForCraft()
    { //어떤 재료 아이템이 있는지 여부 한종류만 체크
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        ready_Craft = false;
        for (int x = 0; x < BackPackController.instance.slots.Length; x++) // 백팩 슬롯의 갯수 만큼
        {
            
            Debug.Log($"첫번째 재료 탐색");
            if(BackPackController.instance.slots[x].item == null)
            {
                Debug.Log("이 슬롯엔 그 아이템이 없습니다.");
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
    public void CraftItem()
    {

        if (SelectedSlot_Recipe.instance.slot != null) //레시피의 슬롯을 선택했을 때
        {
            SearchForCraft();

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
