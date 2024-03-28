using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.Events;

public class NavigationManager : Singleton<NavigationManager>
{
    // 전체 게임맵
    public List<GroundList> gameMap;

    // 플레이어가 위치한 타일이 변경되었을 때 발생할 액션
    // 몬스터에서 함수를 등록
    public UnityAction<Ground> OnChangePlayerGround;
    
    // 플레이어가 위치한 좌표의 그라운드
    // 임의 변경이 불가하고 함수 호출로 변경
    private Ground onPlayerGround;

    public void AssginGameMap(List<GroundList> gameMap)
    {
        this.gameMap = gameMap;
    }

    public void EnterPlayerGround(Ground target)
    {
        // 길찾기를 실시하고있는 몬스터, 동물들이 목표지점을 변경해야 함.
        OnChangePlayerGround?.Invoke(target);
    }
}
