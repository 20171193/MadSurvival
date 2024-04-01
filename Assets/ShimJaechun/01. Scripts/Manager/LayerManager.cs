using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager> 
{
    [SerializeField]
    public LayerMask wallLM;

    [SerializeField]
    public LayerMask obstacleLM;

    [SerializeField]
    public LayerMask monsterLM;
}
