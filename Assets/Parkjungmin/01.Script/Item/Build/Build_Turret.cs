using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;

public class Build_Turret : MonoBehaviour
{
    //공격할 타겟의 큐 -> 큐의 순서 = 공격 순위
     Queue<GameObject> monster_Queue = new Queue<GameObject>(); //몬스터 큐 
    /* 
     *  공격가능한 무언가가 범위에 들어왔다면 큐 삽입.
     *  큐에 들어온 순서대로 공격.
     *  타겟이 죽었거나, 범위를 벗어났을 경우 큐에서 제거.
     */
    [SerializeField] SphereCollider attack_Range;
    [SerializeField] GameObject Turret_Head;

    [Header("터렛의 공격 거리 ")]
    [SerializeField] float range_Radius;

    [Header("터렛의 공격 주기")]
    [SerializeField] float attack_Per_Second;

    Coroutine attakcoroutine;
    bool IsAttack;

    Vector3 TargetDir;

    IEnumerator AttackCoroutine()
    {
        //공격 주기만큼 대기.
        yield return new WaitForSeconds(attack_Per_Second);
        IsAttack = true;
        
    }

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
    private void OnTriggerStay(Collider other) //범위 안에 있을 때는 공격 실행.
    {

        if (((1 << other.gameObject.layer) & (Manager.Layer.turretTargetableLM.value)) != 0)
        {
            Debug.Log("Enter");
            if (!monster_Queue.Contains(other.gameObject))
            {
                monster_Queue.Enqueue(other.gameObject);
                Debug.Log($"몬스터 {other.gameObject.name}이 공격 순위 큐에 삽입되었습니다.");
            }
        }
        if (monster_Queue.Count > 0 )
        {
            AttackQueueCheck();
        }
        if (monster_Queue.Count <= 0)
        {
            Turret_Head.transform.forward = gameObject.transform.forward;
        }
    }
    private void OnTriggerExit(Collider other) //범위를 벗어나면 큐 제거.
    {
        if(monster_Queue.Contains(other.gameObject))
        {
            monster_Queue.Dequeue();
            Debug.Log($"큐에서 몬스터 {other.gameObject.name} 제거됨");
            
        }
    }
    void AttackQueueCheck() //큐에 넣은 순서대로 공격하는 로직.
    {
        TargetDir = (monster_Queue.Peek().gameObject.transform.position - Turret_Head.transform.position).normalized;
        Turret_Head.transform.forward = TargetDir;

        attakcoroutine = StartCoroutine(AttackCoroutine());
        if (IsAttack)
        {
            Attack();
        }
    }
    void Attack() //실제 공격
    {
        //공격 기능.
    }

}
