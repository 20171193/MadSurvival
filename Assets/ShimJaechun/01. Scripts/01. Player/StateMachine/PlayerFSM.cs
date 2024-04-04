using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    private StateMachine<Player> fsm;
    public StateMachine<Player> FSM { get { return fsm; } }

    [SerializeField]
    private GameObject buildingImage;
    [SerializeField]
    private GameObject diggingImage;
    [SerializeField]
    private GameObject attackImage;

    private string currentState;

    // FSM 생성
    public void CreateFSM(Player owner)
    {
        fsm = new StateMachine<Player>(owner);

        // 각 상태별 버튼 UI에 표시할 이미지 할당
        PlayerDiggable diggable = new PlayerDiggable(owner);
        diggable.diggingImage = diggingImage;
        fsm.AddState("Diggable", diggable);
        PlayerAttackable attackable = new PlayerAttackable(owner);
        attackable.attackImage = attackImage;   
        fsm.AddState("Attackable", attackable);

        PlayerBuildable buildable = new PlayerBuildable(owner);
        buildable.buildingImage = buildingImage;
        fsm.AddState("Buildable", buildable);

        // 초기 상태를 Diggable상태로 지정
        // 맨손 상태이므로 채굴과 공격만이 가능.
        fsm.Init("Diggable");
    }

    public void ChangeState(string state)
    {
        fsm.ChangeState(state);
    }

    private void Update()
    {
        // 디버깅용
        currentState = fsm.CurState;
        fsm.Update();
    }
    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }
    private void LateUpdate()
    {
        fsm.LateUpdate();
    }
}
