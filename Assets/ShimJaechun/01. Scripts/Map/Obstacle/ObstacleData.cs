using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleData : ScriptableObject
{
    [Header("이름")]
    public string obstacleName;
    [Header("레벨")]
    public int level;
    [Header("체력")]
    public float hp;
}
