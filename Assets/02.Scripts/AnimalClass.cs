using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//열거형이란 : enum - 
public enum animalType {rabbit, deer, wolf }

public class AnimalClass : MonoBehaviour
{
    //속성
    string animalName;
    int id;
    public animalType animaltype;
    int HP;
    int att;
    int spd;

    //행동
    //이동
    public void Move()
    {

    }
    //공격
    public void Attack() //매개변수는 플레이어
    {

    }

    //아이템을 결정 ( 동물은 완전랜덤으로 아이템 떨굼 )
    public int SetItem() //반환값은 아이템 ID
    {
        return Random.Range(0, 4);
    }

    void Start()
    {
        this.animaltype = (animalType)Random.Range(0, 2);
        gameObject.name = this.animaltype.ToString();
    }
}
