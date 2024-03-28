using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public enum GroundType
{
    Empty,
    Object,
    Wall,
    Water
}

public class Ground : MonoBehaviour
{
    // 건물을 짓는 경우
    public void OnBuild()
    {

    }

    // 플레이어가 타일에 진입한 경우
    public void EnterPlayer()
    {
    }
}

class Monster
{
}
