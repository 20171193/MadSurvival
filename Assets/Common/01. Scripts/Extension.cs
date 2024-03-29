using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Extension method
public static class Extension
{
    // ���̾��ũ�� �ش� ���̾ �����ϰ� �ִ��� üũ
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    // �����ð� ������ ���� �׼� �����Լ�
    public static IEnumerator DelayRoutine(float delayTime, UnityAction action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }
}