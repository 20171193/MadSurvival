using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData_", menuName = "Assets/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("�̸�")]
    public string monsterName;
    [Header("�̵��ӵ�")]
    public float speed;
    [Header("���ݷ�")]
    public float atk;
    [Header("���ݼӵ�")]
    public float ats;
    [Header("ü��")]
    public float hp;
    [Header("����")]
    public float amr;
}
