using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;
using UnityEngine.Events;

public class NavigationManager : Singleton<NavigationManager>
{
    // ��ü ���Ӹ�
    public List<GroundList> gameMap;

    // �÷��̾ ��ġ�� Ÿ���� ����Ǿ��� �� �߻��� �׼�
    // ���Ϳ��� �Լ��� ���
    public UnityAction<Ground> OnChangePlayerGround;
    
    // �÷��̾ ��ġ�� ��ǥ�� �׶���
    // ���� ������ �Ұ��ϰ� �Լ� ȣ��� ����
    private Ground onPlayerGround;

    public void AssginGameMap(List<GroundList> gameMap)
    {
        this.gameMap = gameMap;
    }

    public void EnterPlayerGround(Ground target)
    {
        // ��ã�⸦ �ǽ��ϰ��ִ� ����, �������� ��ǥ������ �����ؾ� ��.
        OnChangePlayerGround?.Invoke(target);
    }
}
