using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Jc
{
    public class Monster : MonoBehaviour, ITileable, IDamageable
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private NavMeshAgent agent;
        public NavMeshAgent Agent { get { return agent; } }

        [SerializeField]
        private Animator anim;
        public Animator Anim { get { return anim; } }

        [Space(3)]
        [Header("Linked Class")]
        [Space(2)]
        [SerializeField]
        private MonsterFSM fsm;
        public MonsterFSM FSM { get { return fsm; } }

        [SerializeField]
        private MonsterDetecter detecter;
        public MonsterDetecter Detecter { get { return detecter; } }

        [Space(3)]
        [Header("Specs")]
        [Space(2)]
        [SerializeField]
        private float speed;
        public float Speed { get { return speed; } set { speed = value; agent.speed = value; } }

        [SerializeField]
        private float hp;
        // ������Ƽ �߰� ����

        [SerializeField]
        private float atk;
        // ������Ƽ �߰� ����

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        // ��ü���� ���� ��
        [SerializeField]
        private List<GroundList> gameMap;
        public List<GroundList> GameMap { get { return gameMap; } }

        // ���Ͱ� ������ Ÿ��
        [SerializeField]
        private Ground onGround;
        public Ground OnGround { get { return onGround; } }

        // �÷��̾ ��ġ�� Ÿ��
        private Ground playerGround;
        public Ground PlayerGround { get { return playerGround; } }

        // ���� �߰��ϴ� ����� �÷��̾����� üũ
        private bool isTrackingPlayer;
        public bool IsTrackingPlayer { get { return isTrackingPlayer; } }

        // ����ȭ
        public NavigationManager Navi => Manager.Navi;

        private void Awake()
        {
            fsm.CreateFSM(this);
            detecter.OnTrigger += DetectTarget;
        }

        private void OnEnable()
        {
            // ���� CSV �����͸� ���� �ε��ؼ� ������ �ӵ��� �Ҵ��ϱ�.

            // �ӵ� �Ҵ�
            agent.speed = speed;

            // �÷��̾��� Ÿ�� ��ġ�� ����� ��� �߻��� �׼�
            Navi.OnChangePlayerGround += OnChangeTarget;
            // ���Ӹ� �Ҵ�
            gameMap = Navi.gameMap;
            playerGround = Navi.OnPlayerGround;
        }

        private void Update()
        {
            anim.SetFloat("MoveSpeed", agent.velocity.magnitude);
        }

        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        // ���� ���� Ʈ������ �׼����� ȣ��
        // Ʈ��ŷ �� �ֺ� ��ü�� ����� ��� ���� 
        public void DetectTarget(GameObject target)
        {
            // �÷��̾� Ž��
            if (isTrackingPlayer && target.tag == "Player" ||
                !isTrackingPlayer && Manager.Layer.wallLM.Contain(target.layer))
            {
                fsm.ChangeState("Attack");
            }
        }

        // ������ ó��
        public void TakeDamage(float damage, Vector3 suspectPos)
        {
            // �������� ó��

            // �˹� ó��
        }

        // ���Ͱ� Ÿ�Ͽ� ������ ��� ����
        public void OnTile(Ground ground)
        {
            onGround = ground;
        }
        // �÷��̾� ��ġ�� ����� ��� ȣ��� �Լ�
        public void OnChangeTarget(Ground playerGround)
        {
            this.playerGround = playerGround;
        }

        #region ���� �߰� �˰���
        // ������ ����
        // �÷��̾ ������ �ѷ��ο� �ִ��� üũ
        //  - true : ���� ����� ������ �̵�
        //  - false : �÷��̾�� �̵�
        public void Tracking(Ground playerGround)
        {
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
            agent.destination = resultTarget.transform.position;

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

                    // ������ ������ �� �ִ� ���� ��ǥ�� ������ ��� (���� �ո� ���)
                    if (nz >= Navi.cornerTL.z || nz <= Navi.cornerBL.z || nx <= Navi.cornerTL.x || nx >= Navi.cornerTR.x)
                        return true;

                    q.Enqueue(new GroundPos(nz, nx));
                    visitied[nz-resol, nx-resol] = true;
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
            Vector3 startPos = new Vector3(onGround.transform.position.x, 0.2f, onGround.transform.position.z);
            Vector3 endPos = new Vector3(playerGround.transform.position.x, 0.2f, onGround.transform.position.z);
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
}