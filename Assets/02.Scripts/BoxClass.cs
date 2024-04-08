using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum boxType { paperBox,woodBox,cabinet,vault }

public class BoxClass : MonoBehaviour
{
    int id;
    public boxType boxType;
    bool isOpen;
    [SerializeField]
    int[] item; //드랍될 수 있는 아이템의 종류(id)
    [SerializeField]
    int size; //사이즈의 크기 == 아이템 드랍되는 개수
    [SerializeField]
    int reqStamina;

    public GameObject itemPrefab;

    //행동
    //아이템을 결정
    public int SetItem() //매개변수는 아이템의 배열(자기 자신의 속성이므로 받을 필요 없음), 반환값은 아이템 id
    {
        return item[Random.Range(0, item.Length - 1)];
    }

    void Start()
    {
        gameObject.name = this.boxType.ToString();
    }
}
