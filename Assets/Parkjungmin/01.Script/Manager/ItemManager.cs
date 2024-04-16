using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Events;
using JetBrains.Annotations;
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
    public Dictionary<string, Item> ItemDic = new Dictionary<string, Item>();
    bool ready_Craft;

    private void Start()
    {
        ItemDicInit();
    }

    //Method : 게임 시작 시 아이템 딕셔너리 초기화
    void ItemDicInit()
    {
        Item[] items = Resources.LoadAll<Item>("ItemDic");
        foreach (Item item in items)
        {
            string name = item.itemdata.itemName;
            Debug.Log($"{name}");
            if (ItemDic.ContainsKey(name)) { continue; } // 만약 딕셔너리가 이 아이템을 이미 포함하고 있었다면 제외.
            ItemDic.Add(name, item);

        }
    }

    // Method : 크래프팅 전에 인벤토리에 충분한 재료 소지 여부 확인 ****
    void SearchForCraft()
    {
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        ready_Craft = false;
        Slot[] invenslots = BackPackController.instance.slots;
        Slot[] quickslots = QuickSlotController.instance.slots;

        //1번 재료 퀵슬롯 체크
        for (int x = 0; x < quickslots.Length; x++)
        {

            if (quickslots[x].item == null)
            {
                continue;
            }
            if (quickslots[x].item != null)
            {
                if (quickslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name)
                {
                    if (quickslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                    {
                        IGD_1_Check = true;
                        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                        { //다음 재료가 존재하면.
                            break;
                        }
                        else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == null && SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                        { //2,3번 조합식이 없을 경우.
                            ready_Craft = true;
                        }
                        break;
                    }
                    else //재료 개수가 부족하면.
                    {
                        IGD_1_Check = false;
                        break;
                    }
                }
            }

        }
        //1번 재료 인벤토리 체크
        if (!IGD_1_Check)
        {
            for (int x = 0; x < invenslots.Length; x++)
            {
                if (invenslots[x].item == null)
                {
                    continue;
                }

                if (invenslots[x].item != null)
                {
                    if (invenslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name)
                    {
                        if (invenslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                        {

                            IGD_1_Check = true;
                            if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                            { //다음 재료가 존재하면.
                                break;
                            }
                            else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == null && SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                            { //2,3번 조합식이 없을 경우
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
        }
        //2번 재료 퀵슬롯 체크
        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null && IGD_1_Check)
        {
            for (int x = 0; x < quickslots.Length; x++) // 백팩 슬롯의 갯수 만큼
            {

                if (quickslots[x].item == null)
                {
                    continue;
                }
                if (quickslots[x].item != null)
                {
                    if (quickslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name)
                    {
                        if (quickslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
                        {

                            IGD_2_Check = true;
                            if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                            {
                                break;
                            }
                            else if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == null)
                            {
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
            //2번 재료 인벤토리 체크.
            if(SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null && (IGD_1_Check && !IGD_2_Check))
            {
                for (int x = 0; x < invenslots.Length; x++) // 백팩 슬롯의 갯수 만큼
                {

                    Debug.Log($"두번째 재료 탐색");
                    if (invenslots[x].item == null)
                    {
                        Debug.Log("이 슬롯엔 그 아이템이 없습니다.");
                        continue;
                    }

                    if (invenslots[x].item != null)
                    {
                        if (invenslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name)
                        {
                            if (invenslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
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
        }

        if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null && IGD_1_Check && IGD_2_Check)
        {
            for (int x = 0; x < quickslots.Length; x++) // 퀵슬롯 슬롯의 갯수 만큼
            {

                if (quickslots[x].item == null)
                {
                    continue;
                }
                if (quickslots[x].item != null)
                {
                    if (quickslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name)
                    {
                        if (quickslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                        {
                            ready_Craft = true;
                            return;
                        }
                    }
                }

            }
            for (int x = 0; x < invenslots.Length; x++) // 백팩 슬롯의 갯수 만큼
            {

                Debug.Log($"세번째 재료 탐색");
                if (invenslots[x].item == null)
                {
                    Debug.Log("이 슬롯엔 그 아이템이 없습니다.");
                    continue;
                }
                if (invenslots[x].item != null)
                {
                    if (invenslots[x].item.itemdata.itemName == SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name)
                    {
                        if (invenslots[x].ItemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                        {

                            ready_Craft = true;
                            return;
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
        bool IGD_1_Check = false;
        bool IGD_2_Check = false;
        bool IGD_3_Check = false;
        Slot[] invenslots = BackPackController.instance.slots;
        Slot[] quickslots = QuickSlotController.instance.slots;


        if (SelectedSlot_Recipe.instance.slot != null)
        {
            SearchForCraft(); // 1. 먼저 재료가 인벤토리 내에 충분히 있는지 확인한다.

            if (ready_Craft) // 2. 재료가 준비가 된 경우
            {
                //2. 조합법의 결과 아이템 인벤토리에 추가.
                BackPackController.instance.AcquireItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe_name]);
                BackPackController.instance.craftsound?.Play();
                Debug.Log("크래프팅 생성");


                //2.2 재료 아이템의 개수의 소모.

                for (int x = 0; x < quickslots.Length; x++) //퀵슬롯의 재료부터 소모.
                {
                    if (quickslots[x].item != null && (SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name == quickslots[x].item.itemdata.itemName))
                    {
                        if (QuickSlotController.instance.slots[x].itemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count)
                        {
                            QuickSlotController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);
                            IGD_1_Check = true;
                        }
                        break;
                    }
                }
                if(!IGD_1_Check)
                {
                    BackPackController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_1.IGD_Count);
                }



                if (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name != null)
                {
                    for (int x = 0; x < quickslots.Length; x++) //퀵슬롯의 재료부터 소모.
                    {
                        if (quickslots[x].item != null && (SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name == quickslots[x].item.itemdata.itemName))
                        {
                            if (QuickSlotController.instance.slots[x].itemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count)
                            {
                                QuickSlotController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                                IGD_2_Check = true;
                            }
                            break;
                        }
                    }
                    if (!IGD_2_Check)
                    {
                        BackPackController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_2.IGD_Count);
                    }

                    if (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name != null)
                    {
                        for (int x = 0; x < quickslots.Length; x++) //퀵슬롯의 재료부터 소모.
                        {
                            if (quickslots[x].item != null && (SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name == quickslots[x].item.itemdata.itemName))
                            {
                                if (QuickSlotController.instance.slots[x].itemCount >= SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count)
                                {
                                    QuickSlotController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                                    IGD_3_Check = true;
                                }
                                break;
                            }
                        }
                        if (!IGD_3_Check)
                        {
                            BackPackController.instance.LoseItem(ItemDic[SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Name], SelectedSlot_Recipe.instance.slot.recipe.IGD_3.IGD_Count);
                        }
                   }

                }

            }
            else if(!ready_Craft)
            {
                    Debug.Log("재료 부족");
            }
        }
    }
}
