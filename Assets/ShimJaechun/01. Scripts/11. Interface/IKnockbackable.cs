using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    public void Knockback(float power, float time, Vector3 suspectPos);
}
