using UnityEngine;

public static class Manager
{
    // JC - Navigation : 게임맵, 플레이어가 위치한 타일 관리자 (몬스터 길찾기 용도로 활용)
    public static NavigationManager Navi { get { return NavigationManager.Instance; } }
    // JC - Layer : 게임 상황에 필요한 레이어들을 담아놓을 LayerMask 관리자
    public static LayerManager Layer { get { return LayerManager.Instance; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // 싱글턴 객체해제
        NavigationManager.ReleaseInstance();
        LayerManager.ReleaseInstance();

        // 싱글턴 객체생성
        NavigationManager.CreateInstance();
        LayerManager.CreateInstance();
    }
}