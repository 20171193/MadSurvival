using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ExplosionTest : MonoBehaviour
{
    [SerializeField]
    Rigidbody[] rigid;

    private void Start()
    {
        foreach(Rigidbody rd in rigid)
        {
           rd.AddExplosionForce(10f, transform.position, 10f);
        }
    }
}
