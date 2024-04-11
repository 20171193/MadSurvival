using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager> 
{
    [Header("빌딩")]
    public LayerMask wallLM;
    [Header("플레이어 공격대상")]
    public LayerMask damageableLM;
    [Header("포탑 공격대상")]
    public LayerMask turretTargetableLM;
    [Header("몬스터")]
    public LayerMask monsterLM;
    [Header("몬스터/동물 공격대상")]
    public LayerMask targetableLM;
    [Header("플레이어")]
    public LayerMask playerLM;
    [Header("플레이어/무적플레이어")]
    public LayerMask playerableLM;
    [Header("드랍아이템")]
    public LayerMask dropItemLM;
    [Header("땅(타일)")]
    public LayerMask groundLM;
    [Header("물(타일)")]
    public LayerMask waterLM;

}
