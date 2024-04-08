using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;

public class Build_Turret : MonoBehaviour
{
    //공격할 타겟의 큐 -> 큐의 순서 = 공격 순위
    Queue<Monster> monster_Queue = new Queue<Monster>(); //몬스터 큐 
    Queue<Animal> animal_Queue= new Queue<Animal>(); //동물 큐
    /* 
     *  공격가능한 무언가가 범위에 들어왔다면 큐 삽입.
     *  큐에 들어온 순서대로 공격.
     *  타겟이 죽었거나, 범위를 벗어났을 경우 큐에서 제거.
     */
    [SerializeField] SphereCollider attack_Range;

    [Header("터렛의 공격 거리 ")]
    [SerializeField] float range_Radius;


    Vector3 TargetDir;

    private void OnEnable()
    {
        if(attack_Range == null) //만약 시작 시 스피어 콜라이더가 부여되어 있지 않다면 자동 부여.
        {
            attack_Range = GetComponent<SphereCollider>();
            
        }
        attack_Range.radius = range_Radius; //활성화 시 인스펙터 창에서 저장한 범위 값을 할당.
        
    }
    private void OnDisable()
    {
        monster_Queue = null;
    }
    private void OnTriggerEnter(Collider other) // 범위 안에 들어왔을 경우 큐 삽입.
    {
        if (other.gameObject.layer == Manager.Layer.turretTargetableLM)
        {
            if (other.gameObject.GetComponent<Monster>() && !monster_Queue.Contains(other.gameObject.GetComponent<Monster>()))
            {
                monster_Queue.Enqueue(other.gameObject.GetComponent<Monster>());
                Debug.Log($"몬스터 {other.gameObject.name}이 공격 순위 큐에 삽입되었습니다.");
            }

            else if (other.gameObject.GetComponent<Animal>() && !animal_Queue.Contains(other.gameObject.GetComponent<Animal>()))
            {
                monster_Queue.Enqueue(other.gameObject.GetComponent<Monster>());
                Debug.Log($"동물 {other.gameObject.name}이 공격 순위 큐에 삽입되었습니다.");
            }
        }
    }
    private void OnTriggerStay(Collider other) //범위 안에 있을 때는 공격 실행.
    {
        //Debug.Log("1??");
        //if (other.gameObject.layer == Manager.Layer.targetableLM)
        //{
        //    Debug.Log("??");
        //    Turret_Attack();
        //}
    }
    private void OnTriggerExit(Collider other) //범위를 벗어나면 큐 제거.
    {
        monster_Queue.Dequeue();
    }
    void Turret_Attack() //큐에 넣은 순서대로 공격하는 로직.
    {
        TargetDir = transform.position - monster_Queue.Peek().gameObject.transform.position;
        Debug.Log("포탑이 공격중");
        transform.forward = TargetDir;
    }

}
