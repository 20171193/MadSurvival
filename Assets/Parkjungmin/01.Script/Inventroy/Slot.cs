using jungmin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler,IPointerClickHandler
{
	Vector3 originPos;
	public ItemData item;
	public int itemCount;
	[SerializeField] public Image itemImage; //���� ���� ������ ������ �̹���.
	[SerializeField] TMP_Text text_Count;
	[SerializeField] GameObject go_CountImage;
	private void Start()
	{
		originPos = transform.position;
	}
	public void AddItem( ItemData _item, int _count = 1 ) //���� ���� ������ UI ������Ʈ
	{
		item = _item;
		itemCount = _count;

        itemImage.sprite = ItemManager.Instance.craftingItemDic[_item.itemName].itemImage;

        if (item.itemtype != ItemData.ItemType.Equipment) //ü��,�Һ� �������̸�,
		{
			go_CountImage.SetActive(true);
			text_Count.text = itemCount.ToString(); 
		}
		else
		{
			text_Count.text = "0";
			go_CountImage.SetActive(false);
		}

		SetColor(1);
	}
	public void SetSlotCount(int _count )
	{
		itemCount += _count;
		text_Count.text = itemCount.ToString();

		if ( itemCount <= 0 )
		{
			ClearSlot();
		}
	}
	public void ClearSlot()
	{
		item = null;
		itemCount = 0;
		itemImage.sprite = GetComponent<Image>().sprite; //������ �⺻ ��������Ʈ�� ����.
		SetColor(0);
		text_Count.text = "0";
		go_CountImage.SetActive(false);

	}
	void SetColor(float alpha ) //������ ������ �̹����� ������ ����
	{
		Color color = itemImage.color;
		color.a = alpha;
		itemImage.color = color;
	}
    void SetColorBG(float alpha) //������ Ʋ �̹����� ������ ����
    {
		Color color = GetComponent<Image>().color;
        color.r = alpha;
		GetComponent<Image>().color = color;
    }

    public void OnBeginDrag( PointerEventData eventData )
	{
		// Ŭ���� ���� �������� �����ϴ���?
		//if ()
		if (BackPackController.inventory_Activated)
		{
			// ���ѿ� �������� �����ϴ� ���
            if (item != null)
            {
                DragSlot.instance.transform.position = eventData.position;
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
            }
        }
	}
	public void OnDrag( PointerEventData eventData )
	{
		// �̹��� �巡��
		if(BackPackController.inventory_Activated)
		{
            if (item != null)
            {
                DragSlot.instance.transform.position = eventData.position;
            }
        }
	}
    public void OnDrop(PointerEventData eventData)
    {
		Debug.Log($"End {this}");

		if (DragSlot.instance.dragSlot != null)
		{
			ChangeSlot();
		}
		else
		{
			Debug.Log("DragSlot.instance.dragSlot is null");
		}
	}

    public void OnEndDrag( PointerEventData eventData )
	{
        Debug.Log($"Start {this}");

        // �巡������ �̹��� ����
        if (BackPackController.inventory_Activated)
		{
			DragSlot.instance.SetColor(0);
			DragSlot.instance.dragSlot = null;

        }
    }
	void ChangeSlot()
	{
		ItemData tempItem = item;
		int tempItemCount = itemCount;

		AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
		if(tempItem != null)
		{
			DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
		}
		else
		{

			DragSlot.instance.dragSlot.ClearSlot();
		}
	}
	void SelectSlot()
	{
        //�κ��丮�� �������� ���� �����ϱ⿡, �κ��丮�� ����.
        // �����Կ��� ���� �����ϱ�
        if (!BackPackController.inventory_Activated)
        {
			if (SelectedSlot.instance.slot != null) //������ ������ ������ �־��ٸ�,
			{
				SelectedSlot.instance.slot.SetColorBG(255);
				Debug.Log("SelectSlot ��ü ����");
				SelectedSlot.instance.slot = this;
				SetColorBG(0);
			}
			else
			{
                SelectedSlot.instance.slot = this;
                SetColorBG(0);
            }
        }

    }
	void ShowToolTip()
    {// �κ��丮�� �������� ���� ����
        if (BackPackController.inventory_Activated)
		{
			//Debug.Log("Show Tool Tip");
		}

	}
    public void OnPointerClick(PointerEventData eventData)
    {
		/* �κ��丮�� �������� �� ������ ���� Ȥ�� ����
		 * �κ��丮�� �������� �� �������� ���� ����
		 * 
		 */
		//Debug.Log("OnPointerClick event");
		if (BackPackController.inventory_Activated)
		{
            ShowToolTip();
            
		}
		else
		{
            SelectSlot();
        }
    }
}
