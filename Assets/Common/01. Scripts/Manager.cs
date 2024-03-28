using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
    // JC - Navigation : ���Ӹ�, �÷��̾ ��ġ�� Ÿ�� ������ (���� ��ã�� �뵵�� Ȱ��)
    public static NavigationManager Navi { get { return NavigationManager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // �̱��� ��ü����
        NavigationManager.ReleaseInstance();

        // �̱��� ��ü����
        NavigationManager.CreateInstance();
    }
}