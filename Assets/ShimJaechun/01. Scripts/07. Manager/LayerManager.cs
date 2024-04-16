using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager> 
{
    [Header("����")]
    public LayerMask wallLM;
    [Header("�÷��̾� ���ݴ��")]
    public LayerMask damageableLM;
    [Header("��ž ���ݴ��")]
    public LayerMask turretTargetableLM;
    [Header("����")]
    public LayerMask monsterLM;
    [Header("����/���� ���ݴ��")]
    public LayerMask targetableLM;
    [Header("�÷��̾�")]
    public LayerMask playerLM;
    [Header("�÷��̾�/�����÷��̾�")]
    public LayerMask playerableLM;
    [Header("���������")]
    public LayerMask dropItemLM;
    [Header("��(Ÿ��)")]
    public LayerMask groundLM;
    [Header("��(Ÿ��)")]
    public LayerMask waterLM;

}
