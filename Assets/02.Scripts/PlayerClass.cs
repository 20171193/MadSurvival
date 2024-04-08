using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerClass : MonoBehaviour
{
    //속성
    string playerName;
    int id;
    int hunger;
    int stamina;
    int HP;
    int att;
    //item[] myItem;
    List<item> myItem;
    item weapon;
    float maxWeight;
    float currentWeight;

    //이동 제어를 위한 속성
    float moveX, moveY;

    //행동
    //이동
    public void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        gameObject.transform.Translate(new Vector3(moveX* 0.01f,0, moveY* 0.01f));
    }

    //탐색
    public int Search(GameObject box) //매개변수는 상자 , 반환값은 아이템 ID
    {
    
        print("찾았음!" + box.name);
        int myItem = box.GetComponent<BoxClass>().SetItem();
        print(myItem);
        Destroy(box);
        return myItem;
    }

    //사냥
    public int Hunt(GameObject animal) //매개변수는 동물, 반환값은 아이템 ID
    {
        print("사냥했음!" + animal.name);
        int myItem = animal.GetComponent<AnimalClass>().SetItem();
        print(myItem);
        Destroy(animal);
        return myItem;
    }

    //아이템 획득
    public item GetItem(GameObject itemObj) //매개변수는 아이템, 반환값은 아이템데이터
    {
        print("아이템 먹었음");
        item addItem = new item();
        addItem.name = itemObj.GetComponent<ItemClass>().itemData.name;
        addItem.id = itemObj.GetComponent<ItemClass>().itemData.id;
        addItem.hungerBonus = itemObj.GetComponent<ItemClass>().itemData.hungerBonus;
        addItem.staminaBonus = itemObj.GetComponent<ItemClass>().itemData.staminaBonus;
        addItem.hpBonus = itemObj.GetComponent<ItemClass>().itemData.hpBonus;
        addItem.attBonus = itemObj.GetComponent<ItemClass>().itemData.attBonus;
        addItem.weight = itemObj.GetComponent<ItemClass>().itemData.weight;
        print(addItem.name);
        Destroy(itemObj);
        return addItem;
    }

    //아이템 사용
    public void UseItem() //매개변수는 아이템,반환값은 없음
    {
        print("아이템 썼음!");
        //아이템 효과 적용
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Animal")
        {
            //other.gameObject.GetComponent<AnimalClass>().SetItem();            
            GameManager.Instance.generateItem(Hunt(other.gameObject),other.gameObject);
        }
        else if (other.gameObject.tag == "Box")
        {
            //other.gameObject.GetComponent<BoxClass>().SetItem();
            GameManager.Instance.generateItem(Search(other.gameObject), other.gameObject);            
        }
        else if(other.gameObject.tag == "Item" && other.gameObject.GetComponent<ItemClass>().isTouched == false)
        {
            if(other.gameObject != null)
            {
                other.gameObject.GetComponent<ItemClass>().isTouched = true;
                myItem.Add(GetItem(other.gameObject));
            }
        }
    }
}