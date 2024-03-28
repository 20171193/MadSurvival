using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
    // JC - Navigation : 게임맵, 플레이어가 위치한 타일 관리자 (몬스터 길찾기 용도로 활용)
    public static NavigationManager Navi { get { return NavigationManager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        NavigationManager.ReleaseInstance();

        // 싱글턴 객체생성
        NavigationManager.CreateInstance();
    }
}