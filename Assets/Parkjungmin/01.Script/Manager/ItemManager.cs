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
        //�������� ����� ���,�κ��丮 â�� Craft ��ư �̺�Ʈ�� ����� ����ȴ�.
    {
        /* 0. ���赵 ������ Ư�� ���� ����
         * 1. Craft ��ư ����
         * 2. ���� �迭�� Ȯ���ϸ鼭, ������� �������� ������ ���� ������ �ִ��� Ȯ��
         * 3. �����ٸ�, ���и� ���ų� �׳� ���õǵ���
         * 4. �����Ѵٸ� �ش� �������� AcquireItem �Լ��� ���� �κ��丮�� �߰���
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
