using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jc;

public class Build_Turret : MonoBehaviour
{
    //������ Ÿ���� ť -> ť�� ���� = ���� ����
    Queue<Monster> monster_Queue = new Queue<Monster>(); //���� ť 
    Queue<Animal> animal_Queue= new Queue<Animal>(); //���� ť
    /* 
     *  ���ݰ����� ���𰡰� ������ ���Դٸ� ť ����.
     *  ť�� ���� ������� ����.
     *  Ÿ���� �׾��ų�, ������ ����� ��� ť���� ����.
     */
    [SerializeField] SphereCollider attack_Range;

    [Header("�ͷ��� ���� �Ÿ� ")]
    [SerializeField] float range_Radius;


    Vector3 TargetDir;

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
    private void OnTriggerEnter(Collider other) // ���� �ȿ� ������ ��� ť ����.
    {
        if (other.gameObject.layer == Manager.Layer.turretTargetableLM)
        {
            if (other.gameObject.GetComponent<Monster>() && !monster_Queue.Contains(other.gameObject.GetComponent<Monster>()))
            {
                monster_Queue.Enqueue(other.gameObject.GetComponent<Monster>());
                Debug.Log($"���� {other.gameObject.name}�� ���� ���� ť�� ���ԵǾ����ϴ�.");
            }

            else if (other.gameObject.GetComponent<Animal>() && !animal_Queue.Contains(other.gameObject.GetComponent<Animal>()))
            {
                monster_Queue.Enqueue(other.gameObject.GetComponent<Monster>());
                Debug.Log($"���� {other.gameObject.name}�� ���� ���� ť�� ���ԵǾ����ϴ�.");
            }
        }
    }
    private void OnTriggerStay(Collider other) //���� �ȿ� ���� ���� ���� ����.
    {
        //Debug.Log("1??");
        //if (other.gameObject.layer == Manager.Layer.targetableLM)
        //{
        //    Debug.Log("??");
        //    Turret_Attack();
        //}
    }
    private void OnTriggerExit(Collider other) //������ ����� ť ����.
    {
        monster_Queue.Dequeue();
    }
    void Turret_Attack() //ť�� ���� ������� �����ϴ� ����.
    {
        TargetDir = transform.position - monster_Queue.Peek().gameObject.transform.position;
        Debug.Log("��ž�� ������");
        transform.forward = TargetDir;
    }

}
