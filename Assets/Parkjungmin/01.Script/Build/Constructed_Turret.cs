using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;
using Unity.VisualScripting;

public class Constructed_Turret : PooledObject
{
    //공격할 타겟의 큐 -> 큐의 순서 = 공격 순위
    [SerializeField]
    List<GameObject> target_List = new List<GameObject>(); //몬스터 큐 
    /* 
     *  공격가능한 무언가가 범위에 들어왔다면 큐 삽입.
     *  큐에 들어온 순서대로 공격.
     *  타겟이 죽었거나, 범위를 벗어났을 경우 큐에서 제거.
     */

    [SerializeField] SphereCollider attack_Range;
    [SerializeField] GameObject Turret_Head;
    [SerializeField] Turret_Line_Renderer turret_Line_Renderer;

    [Header("터렛의 데미지")]
    [SerializeField] float damageValue;
    [Header("터렛의 공격 거리 ")]
    [SerializeField] float range_Radius;

    [Header("터렛의 공격 주기")]
    [SerializeField] float attackCycle_Time;
    [SerializeField] float nowCyle_Time;
    [Header("터렛의 포신 회전 속도")]
    [SerializeField] float rotate_Speed;
    Coroutine attakcoroutine;
    bool IsAttack;

    Vector3 TargetDir;
    public Vector3 TargetPos;

    void Start()
    {
        nowCyle_Time = attackCycle_Time;
    }

    private void OnEnable()
    {
        if (attack_Range == null) //만약 시작 시 스피어 콜라이더가 부여되어 있지 않다면 자동 부여.
        {
            attack_Range = GetComponent<SphereCollider>();
        }
        attack_Range.radius = range_Radius; //활성화 시 인스펙터 창에서 저장한 범위 값을 할당.

    }
    private void OnTriggerEnter(Collider other)
    {
        CheckMonsterLM(other);
    }
    private void OnTriggerExit(Collider other) //범위를 벗어나면 리스트에서 제거.
    {
        if (target_List.Count < 0) return;

        if (target_List.Contains(other.gameObject))
        {
            target_List.Remove(other.gameObject);
            if (target_List.Count <= 0) //대상이 없을 경우 터렛 포신방향 원위치.
            {
                Turret_Head.transform.forward = gameObject.transform.forward;
                if (attakcoroutine != null)
                {
                    StopCoroutine(attakcoroutine);
                    attakcoroutine = null;
                }
            }
        }
    }

    void Attack() //실제 공격
    {
        if (target_List.Count <= 0) return;

        // 포신 회전
        TurretRotation();

        // 공격 로직

        turret_Line_Renderer.TargetToLine(TargetPos); //사격시 총알 궤적이 적을 가리킴
        DamageToEnemy();
        turret_Line_Renderer.gameObject.SetActive(true);
        nowCyle_Time = attackCycle_Time;
        IsAttack = false;
    }
    void TurretRotation() //큐에 넣은 순서대로 공격하는 로직.
    {
        if (target_List.Count <= 0) return;

        TargetPos = target_List[0].gameObject.transform.position; 
        TargetDir = (target_List[0].gameObject.transform.position - Turret_Head.transform.position).normalized;

        //if(Turret_Head.transform.forward != TargetDir.normalized)
        //{
        //    Turret_Head.transform.Rotate(new Vector3(0,TargetDir.y,0) * rotate_Speed * Time.deltaTime);

        //}
        Turret_Head.transform.forward = TargetDir;


    }
    IEnumerator AttackCoroutine()
    {
        //공격 주기만큼 대기.
        while (target_List.Count > 0)
        {
            Attack();
            yield return new WaitForSeconds(attackCycle_Time);
        }

        attakcoroutine = null;
        yield return null;
    }

    void DamageToEnemy()
    {
        if (target_List.Count <= 0) return;


        else if (target_List.Count > 0)
        {
            Debug.Log("Damage");
            IDamageable damageable = target_List[0].GetComponent<IDamageable>();
            damageable.TakeDamage(damageValue);

            if (target_List[0] == null || target_List[0].activeSelf == false)
            {
                target_List.RemoveAt(0);
            }
        }
    }
    void CheckMonsterLM(Collider other)
    {
        if (((1 << other.gameObject.layer) & (Manager.Layer.turretTargetableLM.value)) != 0)
        {
            if (!target_List.Contains(other.gameObject) && (other.gameObject.GetComponent<MonsterStat>() || other.gameObject.GetComponent<AnimalStat>()))
            {
                target_List.Add(other.gameObject);
                Debug.Log($"몬스터 {other.gameObject.name}이 목표 리스트에 삽입되었습니다.");
                if (attakcoroutine == null)
                    attakcoroutine = StartCoroutine(AttackCoroutine());
            }
        }
    }

    void Check_List(List<GameObject> list)
    {
        if (target_List[0] == null || target_List[0].activeSelf == false)
        {
            target_List.RemoveAt(0);
        }
    }

}
