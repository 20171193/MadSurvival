using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;
using Unity.VisualScripting;
using UnityEngine.Events;

// ����õ ���� ��� -> Construct
public class Constructed_Turret : Construct, ITileable, IDamageable
{
    //������ Ÿ���� ť -> ť�� ���� = ���� ����
    [SerializeField]
    public List<GameObject> target_List = new List<GameObject>(); //���� ť 
    [SerializeField]
    private Ground onGround;
    [SerializeField]
    public AudioSource DamagedSound;
    [SerializeField] Fire_Sound firesound;

    [Header("�ͷ��� ü��")]
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

    [Header("�ͷ��� ������")]
    [SerializeField] float damageValue;
    [Header("�ͷ��� ���� �Ÿ� ")]
    [SerializeField] float range_Radius;

    [Header("�ͷ��� ���� �ֱ�")]
    [SerializeField] float attackCycle_Time;
    [SerializeField] float nowCyle_Time;
    //[Header("�ͷ��� ���� ȸ�� �ӵ�")]
    //[SerializeField] float rotate_Speed;
    public Coroutine attakcoroutine;
    bool IsAttack;

    Vector3 TargetDir;
    public Vector3 TargetPos;



    void Start()
    {
        nowCyle_Time = attackCycle_Time;
        attack_Range.radius = range_Radius; //Ȱ��ȭ �� �ν����� â���� ������ ���� ���� �Ҵ�.

    }
    void Attack() //���� ����
    {
        if (target_List.Count <= 0) return;

        for (int x = 0; x < target_List.Count; x++)
        {
            if (target_List[x] == null || target_List[x].activeSelf == false)
            {
                target_List.RemoveAt(x);
            }
        }

        // ���� ȸ��
        TurretRotation();

        // ���� ����

        turret_Line_Renderer.TargetToLine(TargetPos); //��ݽ� �Ѿ� ������ ���� ����Ŵ
        DamageToEnemy();
        turret_Line_Renderer.gameObject.SetActive(true);
        nowCyle_Time = attackCycle_Time;
        IsAttack = false;
    }
    void TurretRotation() //ť�� ���� ������� �����ϴ� ����.
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
        //���� �ֱ⸸ŭ ���.
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
                Debug.Log($"���� {other.gameObject.name}�� ��ǥ ����Ʈ�� ���ԵǾ����ϴ�.");
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
        // ����õ �߰�
        // ����/�������� ��ϵ� �Լ� ����
        // ��ã�⿡ Ȱ��
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
