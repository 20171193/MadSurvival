using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.Events;

public class NavigationManager : Singleton<NavigationManager>
{
    // ��ü ���Ӹ�
    public List<GroundList> gameMap;

    // ��ǥ�� ���� ũ��
    public int mapZsize;
    public int mapXsize;

    // �÷��̾ ���� ���� �� �ִ� ������ 9���ҵ� ���� ��� ����
    // �ش� ��� ����(���簢��)�� �� �𼭸� ��ǥ
    public GroundPos cornerTL;  // �»�� ��ǥ
    public GroundPos cornerTR;  // ���� ��ǥ
    public GroundPos cornerBL;  // ���ϴ� ��ǥ
    public GroundPos cornerBR;  // ���ϴ� ��ǥ

    // �÷��̾ ��ġ�� Ÿ���� ����Ǿ��� �� �߻��� �׼�
    // ���Ϳ��� �Լ��� ���
    public UnityAction<Ground> OnChangePlayerGround;

    // �÷��̾ ��ġ�� ��ǥ�� �׶���
    // ���� ������ �Ұ��ϰ� �Լ� ȣ��� ����
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
        // ��ã�⸦ �ǽ��ϰ��ִ� ����, �������� ��ǥ������ �����ؾ� ��.
        OnChangePlayerGround?.Invoke(target);
    }
}
