using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;
using Unity.VisualScripting;
using UnityEngine.Events;

// 심재천 수정 상속 -> Construct
public class Constructed_Turret : Construct, ITileable, IDamageable
{
    //공격할 타겟의 큐 -> 큐의 순서 = 공격 순위
    [SerializeField]
    public List<GameObject> target_List = new List<GameObject>(); //몬스터 큐 
    [SerializeField]
    private Ground onGround;
    [SerializeField]
    public AudioSource DamagedSound;
    [SerializeField] Fire_Sound firesound;

    [Header("터렛의 체력")]
    public float maxHp;
    public float ownHp;

    public float OwnHp
    {
        get
        {
            return ownHp;
        }
        set
        {
            ownHp = value;
            DamagedSound?.Play();
            if(ownHp <= 0)
            {
                Release();
            }
        }
    }


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
    //[Header("터렛의 포신 회전 속도")]
    //[SerializeField] float rotate_Speed;
    public Coroutine attakcoroutine;
    bool IsAttack;

    Vector3 TargetDir;
    public Vector3 TargetPos;



    void Start()
    {
        nowCyle_Time = attackCycle_Time;
        attack_Range.radius = range_Radius; //활성화 시 인스펙터 창에서 저장한 범위 값을 할당.

    }
    void Attack() //실제 공격
    {
        if (target_List.Count <= 0) return;

        for (int x = 0; x < target_List.Count; x++)
        {
            if (target_List[x] == null || target_List[x].activeSelf == false)
            {
                target_List.RemoveAt(x);
            }
        }

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

        for (int x = 0; x < target_List.Count; x++)
        {
            if (target_List[x] == null || target_List[x].activeSelf == false)
            {
                target_List.RemoveAt(x);
            }
        }

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
            for (int x = 0; x < target_List.Count; x++)
            {
                if (target_List[x] == null || target_List[x].activeSelf == false)
                {
                    target_List.RemoveAt(x);
                }
            }
            IDamageable damageable = target_List[0].GetComponent<IDamageable>();
            damageable.TakeDamage(damageValue);

            if (target_List[0] == null || target_List[0].activeSelf == false)
            {
                target_List.RemoveAt(0);
            }
            firesound?.PlayFire();
        }

    }
    public void CheckMonsterLM(Collider other)
    {
        if ((1 << other.gameObject.layer & Manager.Layer.turretTargetableLM.value) != 0)
        {
            if (!target_List.Contains(other.gameObject) && (other.gameObject.GetComponent<MonsterStat>() || other.gameObject.GetComponent<AnimalStat>()))
            {
                target_List.Add(other.gameObject);
                Debug.Log($"몬스터 {other.gameObject.name}이 목표 리스트에 삽입되었습니다.");
                if (attakcoroutine == null)
                    attakcoroutine = StartCoroutine(AttackCoroutine());

                for(int x = 0; x < target_List.Count; x++)
                {
                    if (target_List[x] == null || target_List[x].activeSelf == false)
                    {
                        target_List.RemoveAt(x);
                    }
                }

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

    public void OnTile(Ground ground)
    {
        onGround = ground;
        onGround.type = GroundType.Wall;
    }
    public Ground GetOnTile()
    {
        return onGround;
    }
    public override void Release()
    {
        // 심재천 추가
        // 몬스터/동물에서 등록된 함수 실행
        // 길찾기에 활용
        OnDestroyWall?.Invoke(gameObject);

        OwnHp = maxHp;
        onGround.type = GroundType.Buildable;
        onGround = null;
        base.Release();
    }

    public void TakeDamage(float damage)
    {
        OwnHp -= damage;
    }
}
