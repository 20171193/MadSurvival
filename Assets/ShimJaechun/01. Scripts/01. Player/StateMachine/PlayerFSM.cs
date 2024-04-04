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

    // FSM ����
    public void CreateFSM(Player owner)
    {
        fsm = new StateMachine<Player>(owner);

        // �� ���º� ��ư UI�� ǥ���� �̹��� �Ҵ�
        PlayerDiggable diggable = new PlayerDiggable(owner);
        diggable.diggingImage = diggingImage;
        fsm.AddState("Diggable", diggable);
        PlayerAttackable attackable = new PlayerAttackable(owner);
        attackable.attackImage = attackImage;   
        fsm.AddState("Attackable", attackable);

        PlayerBuildable buildable = new PlayerBuildable(owner);
        buildable.buildingImage = buildingImage;
        fsm.AddState("Buildable", buildable);

        // �ʱ� ���¸� Diggable���·� ����
        // �Ǽ� �����̹Ƿ� ä���� ���ݸ��� ����.
        fsm.Init("Diggable");
    }

    public void ChangeState(string state)
    {
        fsm.ChangeState(state);
    }

    private void Update()
    {
        // ������
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
