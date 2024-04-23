using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Detector : MonoBehaviour
{
    [SerializeField] Constructed_Turret turret_Parent;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        turret_Parent.CheckMonsterLM(other);
    }
    private void OnTriggerExit(Collider other) //������ ����� ����Ʈ���� ����.
    {
        if (turret_Parent.target_List.Count < 0) return;

        if (turret_Parent.target_List.Contains(other.gameObject))
        {
            turret_Parent.target_List.Remove(other.gameObject);
            if (turret_Parent.target_List.Count <= 0) //����� ���� ��� �ͷ� ���Ź��� ����ġ.
            {
                transform.forward = gameObject.transform.forward;
                if (turret_Parent.attakcoroutine != null)
                {
                    StopCoroutine(turret_Parent.attakcoroutine);
                    turret_Parent.attakcoroutine = null;
                }
            }
        }
    }
}
