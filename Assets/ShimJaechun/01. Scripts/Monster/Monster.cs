using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Jc
{
    public class Monster : MonoBehaviour, ITrackable
    {
        // ��ü���� ���� ��
        [SerializeField]
        private List<GroundList> gameMap;

        // ���Ͱ� ������ Ÿ��
        public Ground onGround;

        private Ground playerGround;

        // ����ȭ
        private NavigationManager Navi => Manager.Navi;

        private void OnEnable()
        {
            // �÷��̾��� Ÿ�� ��ġ�� ����� ��� �߻��� �׼�
            Manager.Navi.OnChangePlayerGround += OnChangeTarget;
            // ���Ӹ� �Ҵ�
            gameMap = Manager.Navi.gameMap;
        }
        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        // ���Ͱ� Ÿ�Ͽ� ������ ��� ����
        public void OnGround(Ground ground)
        {
            onGround = ground;
        }

        public void OnChangeTarget(Ground playerGround)
        {

        }

        // ������ ����
        // �÷��̾ ������ �ѷ��ο� �ִ��� üũ
        //  - true : ���� ����� ������ �̵�
        //  - false : �÷��̾�� �̵�
        private Ground TargetSetting()
        {
            // ����ó�� : �÷��̾ Ÿ�� ���� ��ġ�������� ���
            if (playerGround == null)
            {
                Debug.Log("�÷��̾ ��ġ�� Ÿ���� �������� �ʽ��ϴ�.");
                return null;
            }
            return null;
        }

        // ������ ������ �� �ִ� ��ǥ���� Ž��
        private Ground CheckPlayerBaseCamp()
        {
            int zPos = playerGround.Pos.z;
            int xPos = playerGround.Pos.x;

            // �÷��̾ ������ ������ �� ���� ������ �����ϴ� ���
            // -> �������� �÷��̾ ��ġ�� Ÿ�Ϸ� ����.
            if (zPos > Navi.cornerTL.z || zPos < Navi.cornerBL.z || xPos < Navi.cornerTL.x || xPos > Navi.cornerTR.x)
                return playerGround;

            // �÷��̾ ������ ������ �� �ִ� ������ �����ϴ� ���
            // -> üũ : �÷��̾ ������ �ѷ��ο��ִ���?

            // BFS Ž��
            Queue<GroundPos> q = new Queue<GroundPos>();
            // �湮Ȯ�� �迭 ���� (���� ����)
            bool[,] visitied = new bool[Navi.mapZsize/3,Navi.mapXsize/3];
            int resol = Navi.mapZsize / 3 - 1;
            // 4���� Ž���� ���� ����
            int[] dz = { 0, 0, 1, -1 };
            int[] dx = { 1, -1, 0, 0 };
            q.Enqueue(playerGround.Pos);
            // �湮�迭 Ȯ���� ���� �׶��� ��ġ�� ������ǥ�� ��ȯ
            visitied[playerGround.Pos.z - resol, playerGround.Pos.x - resol] = true;

            while(q.Count > 0)
            {
                GroundPos curPos = q.Dequeue();
                for(int i =0; i<4; i++)
                {
                    int nz = dz[i] + curPos.z;
                    int nx = dx[i] + curPos.x;

                    // ����ó�� 
                    if (nz > Navi.cornerTL.z || nz < Navi.cornerBL.z || nx < Navi.cornerTL.x || nx > Navi.cornerTR.x) continue;
                    if (visitied[nz - resol, nx - resol]) continue;
                    if (Navi.gameMap[nz].groundList[nx].type != GroundType.Buildable) continue;

                    // ���� �ո� ���
                    if (nz >= Navi.cornerTL.z || nz <= Navi.cornerBL.z || nx <= Navi.cornerTL.x || nx >= Navi.cornerTR.x)
                        return playerGround;

                    q.Enqueue(new GroundPos(nz, nx));
                    visitied[nz-resol, nx-resol] = true;
                }
            }

            // ���� �ո��� ���� ��� ���� ��ġ���� ���� ����� ���� �������� ����
            // ������ġ -> �÷��̾���ġ ����ĳ����
            Vector3 startPos = new Vector3(onGround.transform.position.x, 0.2f, onGround.transform.position.z);
            Vector3 endPos = new Vector3(playerGround.transform.position.x, 0.2f, onGround.transform.position.z);
            Debug.DrawLine(startPos, endPos, Color.red, 0.5f);
            if (Physics.Raycast(new Ray(startPos, endPos), out RaycastHit hitInfo, (endPos - startPos).magnitude, Manager.Layer.wallLM))
            {
                if(hitInfo.transform.GetComponent<Ground>())
                {
                    
                }

                return hitInfo.transform.GetComponent<Ground>();
            }
            return null;
        }
    }
}