using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    void Awake()
    {
        Instance = this; 
    }
    //게임매니저가 게임의 요소를 통제하려면
    //게임매니저가 게임의 모든 요소를 속성(멤버변수)로 들고 있으면 된다.

    //프리팹(클래스)들
    public GameObject playerPrefab;
    public GameObject animalPrefab;
    public GameObject boxPrefab;
    public GameObject itemPrefab;

    //게임 안에서 사용할 전체 오브젝트들
    GameObject player;
    GameObject[] animals;
    GameObject[] boxes;
    GameObject[] items;
    item[] itemData;

    //게임 안에서 사용할 전체 UI 오브젝트들

    // Start is called before the first frame update
    void Start()
    {
        InitializeGameData();        
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 이동
        player.GetComponent<PlayerClass>().Move();
    }

    void InitializeGameData() //게임 데이터 초기화
    {
        //아이템데이터리스트 설정
        itemData = new item[5];
        itemData[0] = new item("치킨",0,10,0,0,0,0.5f);
        itemData[1] = new item("짱돌",1,0,0,0,3,1.0f);
        itemData[2] = new item("커피",2,3,10,0,0,0.3f);
        itemData[3] = new item("소독약",3,0,0,10,0,0.7f);
        itemData[4] = new item("녹용",4,5,5,5,5,3.0f);

        /*
         지금은 야매라 이렇게 하지만 원래는 데이터를 읽어와서 설정해야 함
         */
        //플레이어 오브젝트 연결
        player = GameObject.FindGameObjectWithTag("Player");

        //상자 오브젝트 연결
        boxes = GameObject.FindGameObjectsWithTag("Box");

        //동물 세 마리 생성
        generateAnimal(3);
        
    }

    void InitializeUI() //UI 초기화
    {

    }
    void generateAnimal(int amount) //동물 생성 (매개변수 : 동물 수 )
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject instantAnimal = Instantiate(animalPrefab);
            instantAnimal.transform.position = new Vector3(Random.Range(0, 5), 0.5f, Random.Range(0, 5));
        }
        animals = GameObject.FindGameObjectsWithTag("Animal");
    }

    public void generateItem(int itemID,GameObject other)
    {        
        GameObject instantItem = Instantiate(itemPrefab);
        instantItem.GetComponent<ItemClass>().itemData = itemData[itemID];
        instantItem.name = instantItem.GetComponent<ItemClass>().itemData.name;
        instantItem.transform.position = other.transform.position;
        print("아이템은" + instantItem.name);
    }

}

public class item
{
    public string name;
    public int id;
    public int hungerBonus;
    public int staminaBonus;
    public int hpBonus;
    public int attBonus;
    public float weight;

    public item()
    {

    }

    public item(string name, int id, int hungerBonus, int staminaBonus, int hpBonus, int attBonus, float weight)
    {
        this.name = name;
        this.id = id;
        this.hungerBonus = hungerBonus;
        this.staminaBonus = staminaBonus;
        this.hpBonus = hpBonus;
        this.attBonus = attBonus;
        this.weight = weight;
    }
}