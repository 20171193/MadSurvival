using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.Events;

public class NavigationManager : Singleton<NavigationManager>
{
    // 전체 게임맵
    public List<GroundList> gameMap;

    // 좌표별 맵의 크기
    public int mapZsize;
    public int mapXsize;

    // 플레이어가 벽을 지을 수 있는 공간은 9분할된 맵의 가운데 공간
    // 해당 가운데 공간(정사각형)의 각 모서리 좌표
    public GroundPos cornerTL;  // 좌상단 좌표
    public GroundPos cornerTR;  // 우상단 좌표
    public GroundPos cornerBL;  // 좌하단 좌표
    public GroundPos cornerBR;  // 우하단 좌표

    // 플레이어가 위치한 타일이 변경되었을 때 발생할 액션
    // 몬스터에서 함수를 등록
    public UnityAction<Ground> OnChangePlayerGround;

    // 플레이어가 위치한 좌표의 그라운드
    // 임의 변경이 불가하고 함수 호출로 변경
    [SerializeField]
    private Ground onPlayerGround;
    public Ground OnPlayerGround { get { return onPlayerGround; } }

    public void AssginGameMap(List<GroundList> gameMap)
    {
        this.gameMap = gameMap;
    }

    public void EnterPlayerGround(Ground target)
    {
        onPlayerGround = target;
        // 길찾기를 실시하고있는 몬스터, 동물들이 목표지점을 변경해야 함.
        OnChangePlayerGround?.Invoke(target);
    }
}
