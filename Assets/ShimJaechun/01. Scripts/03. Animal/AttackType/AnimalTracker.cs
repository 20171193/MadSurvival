using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class AnimalTracker : MonoBehaviour
{
    [SerializeField]
    private AttackAnimal owner;
    public NavigationManager Navi => Manager.Navi;

    // ���� �߰��ϴ� ����� �÷��̾����� üũ
    private bool isTrackingPlayer;
    public bool IsTrackingPlayer { get { return isTrackingPlayer; } }

    #region ���� �߰� �˰���
    // ������ ����
    // �÷��̾ ������ �ѷ��ο� �ִ��� üũ
    //  - true : ���� ����� ������ �̵�
    //  - false : �÷��̾�� �̵�
    public void Tracking(Ground playerGround)
    {
        if (owner.Agent.enabled == false) return;
        // ����ó�� : �÷��̾ Ÿ�� ���� ��ġ�������� ���
        if (playerGround == null)
        {
            Debug.Log("�÷��̾ ��ġ�� Ÿ���� �������� �ʽ��ϴ�.");
            return;
        }
        // ������ ��ǥ (�÷��̾�)
        Ground originTarget = playerGround;
        // Ž�� ��� ��ǥ
        Ground resultTarget = TargetSetting();
        owner.Agent.destination = resultTarget.transform.position;

        // Ž�� ����� �÷��̾��� ��� true
        isTrackingPlayer = originTarget == resultTarget;
    }
    private Ground TargetSetting()
    {
        int zPos = owner.playerGround.Pos.z;
        int xPos = owner.playerGround.Pos.x;

        // �÷��̾ ������ ������ �� ���� ������ �����ϴ� ���
        // -> �������� �÷��̾ ��ġ�� Ÿ�Ϸ� ����.
        if (zPos < Navi.cornerTL.z || zPos > Navi.cornerBL.z || xPos < Navi.cornerTL.x || xPos > Navi.cornerTR.x)
            return owner.playerGround;
        
        // �÷��̾� �ֺ��� ������ �ѷ������� ���� ���
        if (!PlayerInBaseCamp())
            return owner.playerGround;
        // �÷��̾� �ֺ��� ������ �ѷ����� ���
        // ���� ����� ���� ã�� �߰�
        return GetNearWall();
    }
    // �÷��̾ ������ �ѷ��ο��ִ��� üũ
    // ������ ������ �� �ִ� ��ǥ���� Ž�� (�÷��̾� ���� BFS)
    private bool PlayerInBaseCamp()
    {
        // �÷��̾ ������ ������ �� �ִ� ������ �����ϴ� ���
        // -> üũ : �÷��̾ ������ �ѷ��ο��ִ���?

        // BFS Ž��
        Queue<GroundPos> q = new Queue<GroundPos>();
        // �湮Ȯ�� �迭 ���� (���� ����)
        bool[,] visitied = new bool[Navi.mapZsize / 3 + 1, Navi.mapXsize / 3 + 1];
        int resol = Navi.mapZsize / 3 - 1;
        // 4���� Ž���� ���� ����
        int[] dz = { 0, 0, 1, -1 };
        int[] dx = { 1, -1, 0, 0 };
        q.Enqueue(owner.playerGround.Pos);
        // �湮�迭 Ȯ���� ���� �׶��� ��ġ�� ������ǥ�� ��ȯ
        visitied[owner.playerGround.Pos.z - resol, owner.playerGround.Pos.x - resol] = true;

        while (q.Count > 0)
        {
            GroundPos curPos = q.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int nz = dz[i] + curPos.z;
                int nx = dx[i] + curPos.x;

                // ����ó�� 
                if (nz > Navi.cornerTL.z || nz < Navi.cornerBL.z || nx < Navi.cornerTL.x || nx > Navi.cornerTR.x) continue;
                if (visitied[nz - resol, nx - resol]) continue;
                if (Navi.gameMap[nz].groundList[nx].type != GroundType.Buildable) continue;

                // ������ ������ �� �ִ� ���� ��ǥ�� ������ ��� (���� �ո� ���)
                if (nz >= Navi.cornerTL.z || nz <= Navi.cornerBL.z || nx <= Navi.cornerTL.x || nx >= Navi.cornerTR.x)
                    return true;

                q.Enqueue(new GroundPos(nz, nx));
                visitied[nz - resol, nx - resol] = true;
            }
        }
        // ������ �ѷ����� ���
        return false;
    }
    // ����ĳ��Ʈ�� ���� ���� ����� �� ã��
    private Ground GetNearWall()
    {
        // ���� ��ġ���� ���� ����� ���� �������� ����
        // ������ġ -> �÷��̾���ġ ����ĳ���� [LayerMask = ��]
        Vector3 startPos = new Vector3(owner.onGround.transform.position.x, 0.2f, owner.onGround.transform.position.z);
        Vector3 endPos = new Vector3(owner.playerGround.transform.position.x, 0.2f, owner.onGround.transform.position.z);
        Debug.DrawLine(startPos, endPos, Color.red, 0.5f);

        // ���� ����� ������ �̵� 
        if (Physics.Raycast(new Ray(startPos, endPos), out RaycastHit hitInfo, (endPos - startPos).magnitude, Manager.Layer.wallLM))
        {
            Wall targetWall = hitInfo.transform.GetComponent<Wall>();
            return targetWall?.OnGround;
        }

        Debug.Log($"{this.gameObject} can't find wall");
        return null;
    }
    #endregion

}
