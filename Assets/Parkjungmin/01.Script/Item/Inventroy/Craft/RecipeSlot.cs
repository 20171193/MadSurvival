using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{
    public Item resultItem; //결과 아이템
    public Recipe recipe;
    public string recipe_name;
    public Image bg_image;
    public Image item_image;
    public UnityEvent OnSelect;
    void OnEnable()
    {
        recipe_name = recipe.name;
        resultItem = ItemManager.Instance.craftingItemDic[recipe.name];


        bg_image = GetComponent<Image>();
        item_image.sprite = resultItem.itemdata.itemImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot();
        RecipeController.instance.ShowRecipeInfo();
    }
    public void SetColorBG(float alpha)
    {
        Color color = bg_image.color;
        color.r = alpha;
        bg_image.color = color;
    }
    public void ResetSlot()
    {
        resultItem = ItemManager.Instance.craftingItemDic[recipe.name];
        item_image.sprite = resultItem.itemdata.itemImage;
        recipe_name = recipe.name;
    }
    void SelectSlot()
    {

        if (SelectedSlot_Recipe.instance.slot != null) //이전에 셀렉된 슬롯이 있었다면,
        {
            SelectedSlot_Recipe.instance.slot.SetColorBG(255);
            SelectedSlot_Recipe.instance.slot = this;
            SetColorBG(0);
        }
        else
        {
            SelectedSlot_Recipe.instance.slot = this;
            SetColorBG(0);
        }

    }
}
 