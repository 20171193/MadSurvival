using Jc;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Jc
{
    public class MonsterDetecter : MonoBehaviour
    {
        [SerializeField]
        private Monster owner;

        // ��ü���� ���� ��
        [SerializeField]
        private List<GroundList> gameMap;
        public List<GroundList> GameMap { get { return gameMap; } }

        // ���Ͱ� ������ Ÿ��
        [SerializeField]
        private Ground onGround;
        public Ground OnGround { get { return onGround; } set { onGround = value; } }

        // �÷��̾ ��ġ�� Ÿ��
        [SerializeField]
        private Ground playerGround;
        public Ground PlayerGround { get { return playerGround; } set { playerGround = value; } }

        [SerializeField]
        // ���� �߰��ϴ� ����� �÷��̾����� üũ
        private bool isTrackingPlayer;
        public bool IsTrackingPlayer { get { return isTrackingPlayer; } }

        private GameObject currentTarget = null;
        public GameObject CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }

        // ����ȭ
        public NavigationManager Navi => Manager.Navi;

        // ������ ���� �� �ִ� ������Ʈ�� Ž��
        // �Ű����� : ��ü, ���̾� 
        public UnityAction<GameObject> OnTrigger;
        public UnityAction<GameObject> OffTrigger;

        private void OnEnable()
        {
            gameMap = Navi.gameMap;
        }

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
            int zPos = playerGround.Pos.z;
            int xPos = playerGround.Pos.x;

            // �÷��̾ ������ ������ �� ���� ������ �����ϴ� ���
            // -> �������� �÷��̾ ��ġ�� Ÿ�Ϸ� ����.
            if (zPos < Navi.cornerTL.z || zPos > Navi.cornerBL.z || xPos < Navi.cornerTL.x || xPos > Navi.cornerTR.x)
                return playerGround;

            // �÷��̾� �ֺ��� ������ �ѷ������� ���� ���
            if (!PlayerInBaseCamp())
                return playerGround;
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
            q.Enqueue(playerGround.Pos);
            // �湮�迭 Ȯ���� ���� �׶��� ��ġ�� ������ǥ�� ��ȯ
            visitied[playerGround.Pos.z - resol, playerGround.Pos.x - resol] = true;

            while (q.Count > 0)
            {
                GroundPos curPos = q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int nz = dz[i] + curPos.z;
                    int nx = dx[i] + curPos.x;

                    // ����ó�� 
                    if (nz > Navi.cornerBL.z || nz < Navi.cornerTL.z || nx < Navi.cornerTL.x || nx > Navi.cornerTR.x)
                        continue;
                    if (visitied[nz - resol, nx - resol])
                        continue;
                    if (Navi.gameMap[nz].groundList[nx].type != GroundType.Buildable)
                        continue;

                    // ������ ������ �� �ִ� ���� ��ǥ�� ������ ��� (���� �ո� ���)
                    if (nz <= Navi.cornerTL.z || nz >= Navi.cornerBL.z || nx <= Navi.cornerTL.x || nx >= Navi.cornerTR.x)
                        return false;

                    q.Enqueue(new GroundPos(nz, nx));
                    visitied[nz - resol, nx - resol] = true;
                }
            }

            Debug.Log("�÷��̾�� ���� �����ȿ� ��ġ���ֽ��ϴ�.");
            // ������ �ѷ����� ���
            return true;
        }

        // ����ĳ��Ʈ�� ���� ���� ����� �� ã��
        private Ground GetNearWall()
        {
            // ���� ��ġ���� ���� ����� ���� �������� ����
            // ������ġ -> �÷��̾���ġ ����ĳ���� [LayerMask = ��]
            Vector3 startPos = new Vector3(onGround.transform.position.x, 2f, onGround.transform.position.z);
            Vector3 endPos = new Vector3(playerGround.transform.position.x, 2f, playerGround.transform.position.z);
            Debug.DrawLine(startPos, endPos, Color.red, 10f);

            // ���� ����� �� ã��
            if (Physics.Raycast(startPos, (endPos - startPos).normalized, out RaycastHit hit, 200f, Manager.Layer.wallLM))
            {
                ITileable targetGround = hit.transform.GetComponent<ITileable>();
                return targetGround?.GetOnTile();
            }


            Debug.Log($"{this.gameObject} can't find wall");
            return playerGround;
        }
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            // �������� ���� �� �ִ� ��, ������ ������ ��ü�� ��� �׼� 
            if (other.GetComponent<IDamageable>() != null)
            {
                if (owner.FSM.FSM.CurState == "Tracking" &&
                (isTrackingPlayer && other.gameObject.tag == "Player" ||
                !isTrackingPlayer && Manager.Layer.wallLM.Contain(other.gameObject.layer)))
                {
                    // Ÿ������ ����
                    currentTarget = other.gameObject;

                    owner.FSM.ChangeState("Attack");
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                // ������ Ÿ�� ����
                if (other.gameObject == currentTarget)
                    currentTarget = null;
            }
        }
    }
}