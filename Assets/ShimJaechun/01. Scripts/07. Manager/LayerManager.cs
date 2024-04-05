using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager> 
{
    [SerializeField]
    public LayerMask wallLM;

    [SerializeField]
    public LayerMask damageableLM;

    [SerializeField]
    public LayerMask monsterLM;

    [SerializeField]
    public LayerMask playerLM;

    [SerializeField]
    public LayerMask playerableLM;

    [SerializeField]
    public LayerMask dropItemLM;

    [SerializeField]
    public LayerMask groundLM;
}
