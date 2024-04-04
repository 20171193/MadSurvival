using jungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{
    public Item resultItem; //결과 아이템
    public Recipe recipe;
    public Image bg_image;
    public Image item_image;
    void Start()
    {
        bg_image = GetComponent<Image>();
        item_image.sprite = resultItem.itemImage;
    }

    public void Combine()
    {
        
    }


    public void SetColorBG(float alpha)
    {
        Color color = bg_image.color;
        color.r = alpha;
        bg_image.color = color;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot();
    }
}
