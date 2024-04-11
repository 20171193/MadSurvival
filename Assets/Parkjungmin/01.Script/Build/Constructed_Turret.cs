using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;
using Unity.VisualScripting;

public class Constructed_Turret : PooledObject
{
    //������ Ÿ���� ť -> ť�� ���� = ���� ����
    [SerializeField]
    List<GameObject> target_List = new List<GameObject>(); //���� ť 
    /* 
     *  ���ݰ����� ���𰡰� ������ ���Դٸ� ť ����.
     *  ť�� ���� ������� ����.
     *  Ÿ���� �׾��ų�, ������ ����� ��� ť���� ����.
     */

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
    [Header("�ͷ��� ���� ȸ�� �ӵ�")]
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
        if (attack_Range == null) //���� ���� �� ���Ǿ� �ݶ��̴��� �ο��Ǿ� ���� �ʴٸ� �ڵ� �ο�.
        {
            attack_Range = GetComponent<SphereCollider>();
        }
        attack_Range.radius = range_Radius; //Ȱ��ȭ �� �ν����� â���� ������ ���� ���� �Ҵ�.

    }
    private void OnTriggerEnter(Collider other)
    {
        CheckMonsterLM(other);
    }
    private void OnTriggerExit(Collider other) //������ ����� ����Ʈ���� ����.
    {
        if (target_List.Count < 0) return;

        if (target_List.Contains(other.gameObject))
        {
            target_List.Remove(other.gameObject);
            if (target_List.Count <= 0) //����� ���� ��� �ͷ� ���Ź��� ����ġ.
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

    void Attack() //���� ����
    {
        if (target_List.Count <= 0) return;

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
                Debug.Log($"���� {other.gameObject.name}�� ��ǥ ����Ʈ�� ���ԵǾ����ϴ�.");
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
