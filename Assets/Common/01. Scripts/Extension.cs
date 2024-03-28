using System.Collections;
using UnityEngine;

// Extension method
public static class Extension
{
    // ���̾��ũ�� �ش� ���̾ �����ϰ� �ִ��� üũ
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}