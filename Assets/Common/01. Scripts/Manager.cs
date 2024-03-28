using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
    // 사용예시
    // public static ---Manager Layer { get { return ---Manager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // ---Manager.ReleaseInstance();

        // ---Manager.CreateInstance();
    }
}