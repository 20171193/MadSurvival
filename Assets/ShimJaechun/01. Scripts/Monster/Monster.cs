using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Jc
{
    public class Monster : PooledObject, ITileable, IDamageable
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private Rigidbody rigid;

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
        [Header("�� �����ؾ��ϴ� �κ�. �̸����� Find")]
        [SerializeField]
        private string monsterName;
        public string MonsterName { get { return monsterName; } }

        [SerializeField]
        private float knockBackPower;
        public float KnockBackPower { get { return knockBackPower; } }

        [SerializeField]
        private float knockBackTime;
        public float KnockBackTime { get { return knockBackTime; } }

        private Coroutine knockBackTimer;

        [SerializeField]
        private float speed;
        public float Speed { get { return speed; } set { speed = value; agent.speed = value; } }
        [SerializeField]
        private float atk;
        // ������Ƽ �߰� ����
        public float ATK { get { return atk; } }

        [SerializeField]
        private float ats;
        public float ATS { get { return ats; } }

        [SerializeField]
        private float hp;
        [SerializeField]
        private float ownHp;
        public float OwnHp { get { return ownHp; } }


        [SerializeField]
        private float amr;
        public float AMR { get { return amr; } }

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
        [SerializeField]
        private Ground playerGround;
        public Ground PlayerGround { get { return playerGround; } }

        // ���� �߰��ϴ� ����� �÷��̾����� üũ
        private bool isTrackingPlayer;
        public bool IsTrackingPlayer { get { return isTrackingPlayer; } }

        private GameObject currentTarget = null;
        public GameObject CurrentTarget { get { return currentTarget; } }

        public UnityAction<Monster> OnMonsterDie;

        // ����ȭ
        public NavigationManager Navi => Manager.Navi;

        public string currentState;

        private void Awake()
        {
            fsm.CreateFSM(this);
            detecter.OnTrigger += FindTarget;
            detecter.OffTrigger += LoseTarget;
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
            InitSetting();

            // ���º���
            fsm.ChangeState("Idle");
        }

        private void Update()
        {
            anim.SetFloat("MoveSpeed", agent.velocity.magnitude);
            currentState = fsm.currentState;
        }

        private void OnDisable()
        {
            Manager.Navi.OnChangePlayerGround -= OnChangeTarget;
        }

        // ���� �ʱ⼳�� (������ �ε�)
        private void InitSetting()
        {
            if (!Manager.Data.monsterDataDic.ContainsKey(monsterName))
            {
                Debug.Log($"{monsterName} : �� �����Ͱ� �����ϴ�.");
                return;
            }

            MonsterData loadedData = Manager.Data.monsterDataDic[monsterName];
            Speed = loadedData.speed;
            atk = loadedData.atk;
            ats = loadedData.ats;
            hp = loadedData.hp;
            ownHp = hp; 
            amr = loadedData.amr;
        }

        // ���� ���� Ʈ������ �׼����� ȣ��
        // Ʈ��ŷ �� �ֺ� ��ü�� ����� ��� ���� 
        public void FindTarget(GameObject target)
        {
            // �÷��̾� Ž��
            if (isTrackingPlayer && target.tag == "Player" ||
                !isTrackingPlayer && Manager.Layer.wallLM.Contain(target.layer))
            {
                currentTarget = target;
                fsm.ChangeState("Attack");
            }
        }
        public void LoseTarget(GameObject target)
        {
            if (target == currentTarget)
                currentTarget = null;
        }

        // ������ ó��
        public void TakeDamage(float value, Vector3 suspectPos)
        {
            // �������� ó��
            float damage = value - amr;
            if (damage <= 0) return;

            // ���ó��
            if(ownHp < damage)
            {
                fsm.ChangeState("Die");
            }
            // ������ ó��, �˹�
            else
            {
                ownHp -= damage;
                knockBackTimer = StartCoroutine(KnockBackRoutine());
            }
        }
        IEnumerator KnockBackRoutine()
        {
            // �׺�޽� ��Ȱ��ȭ
            // ���� �̵����� ��ȯ
            agent.enabled = false;
            rigid.AddForce(transform.forward * -knockBackPower, ForceMode.Impulse);

            yield return new WaitForSeconds(KnockBackTime);

            // ����
            rigid.velocity = Vector3.zero;
            agent.enabled = true;
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
            if (agent.enabled == false) return;
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
            bool[,] visitied = new bool[Navi.mapZsize/3+1,Navi.mapXsize/3+1];
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