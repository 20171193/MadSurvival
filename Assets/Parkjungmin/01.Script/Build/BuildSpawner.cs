using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpawner : MonoBehaviour
{

    [SerializeField]
    public Constructed_Turret turret;
    public Constructed_Turret turret3;
    public Constructed_Wall wall;
    

    private void Awake()
    {
        Manager.Pool.CreatePool(turret, turret.Size, turret.Size + 5);
        Manager.Pool.CreatePool(turret3, turret.Size, turret.Size + 5);
        Manager.Pool.CreatePool(wall, wall.Size, wall.Size + 5);
    }
}
