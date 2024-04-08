using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;

public class Build_Turret : MonoBehaviour
{
    //������ Ÿ���� ť -> ť�� ���� = ���� ����
     Queue<GameObject> monster_Queue = new Queue<GameObject>(); //���� ť 
    /* 
     *  ���ݰ����� ���𰡰� ������ ���Դٸ� ť ����.
     *  ť�� ���� ������� ����.
     *  Ÿ���� �׾��ų�, ������ ����� ��� ť���� ����.
     */
    [SerializeField] SphereCollider attack_Range;
    [SerializeField] GameObject Turret_Head;

    [Header("�ͷ��� ���� �Ÿ� ")]
    [SerializeField] float range_Radius;

    [Header("�ͷ��� ���� �ֱ�")]
    [SerializeField] float attack_Per_Second;

    Coroutine attakcoroutine;
    bool IsAttack;

    Vector3 TargetDir;

    IEnumerator AttackCoroutine()
    {
        //���� �ֱ⸸ŭ ���.
        yield return new WaitForSeconds(attack_Per_Second);
        IsAttack = true;
        
    }

    private void OnEnable()
    {
        if(attack_Range == null) //���� ���� �� ���Ǿ� �ݶ��̴��� �ο��Ǿ� ���� �ʴٸ� �ڵ� �ο�.
        {
            attack_Range = GetComponent<SphereCollider>();
            
        }
        attack_Range.radius = range_Radius; //Ȱ��ȭ �� �ν����� â���� ������ ���� ���� �Ҵ�.
        
    }
    private void OnDisable()
    {
        monster_Queue = null;
    }
    private void OnTriggerStay(Collider other) //���� �ȿ� ���� ���� ���� ����.
    {

        if (((1 << other.gameObject.layer) & (Manager.Layer.turretTargetableLM.value)) != 0)
        {
            Debug.Log("Enter");
            if (!monster_Queue.Contains(other.gameObject))
            {
                monster_Queue.Enqueue(other.gameObject);
                Debug.Log($"���� {other.gameObject.name}�� ���� ���� ť�� ���ԵǾ����ϴ�.");
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
    private void OnTriggerExit(Collider other) //������ ����� ť ����.
    {
        if(monster_Queue.Contains(other.gameObject))
        {
            monster_Queue.Dequeue();
            Debug.Log($"ť���� ���� {other.gameObject.name} ���ŵ�");
            
        }
    }
    void AttackQueueCheck() //ť�� ���� ������� �����ϴ� ����.
    {
        TargetDir = (monster_Queue.Peek().gameObject.transform.position - Turret_Head.transform.position).normalized;
        Turret_Head.transform.forward = TargetDir;

        attakcoroutine = StartCoroutine(AttackCoroutine());
        if (IsAttack)
        {
            Attack();
        }
    }
    void Attack() //���� ����
    {
        //���� ���.
    }

}
