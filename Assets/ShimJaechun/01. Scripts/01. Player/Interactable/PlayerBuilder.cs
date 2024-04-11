using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
namespace Jc
{
    public enum BuildDirection
    {
        Front,
        Back,
        Left,
        Right
    }

    public class PlayerBuilder : MonoBehaviour
    {
        [SerializeField]
        private Player owner;
        [SerializeField]
        private GameObject buildSocket;             // ������� ������Ʈ
        [SerializeField]
        private MeshRenderer buildSocketRenderer;   // ������� ������Ʈ ������

        [SerializeField]
        private Material enableSocketMT;    // Ȱ��ȭ �� ���͸���
        [SerializeField]
        private Material disableSocketMT;   // ��Ȱ��ȭ �� ���͸���

        private Ground buildableGround;
        private BuildDirection buildDirection;
        private Coroutine socketSetRoutine;

        private void Awake()
        {
            owner = GetComponent<Player>();
        }

        public void EnterBuildMode()
        {
            if(socketSetRoutine == null)
                socketSetRoutine = StartCoroutine(SocketSetRoutine());
        }
        public void ExitBuildMode()
        {
            if (socketSetRoutine != null)
                StopCoroutine(socketSetRoutine);
        }

        public void Build(Build_Base buildItem)
        {
            if (buildableGround == null || buildItem == null) return;

            buildItem.Build(buildableGround);
        }

        // ���� ��ġ ����
        IEnumerator SocketSetRoutine()
        {
            while(true)
            {
                CheckBuildable();
                yield return new WaitForSeconds(0.1f);
            }
        }
        private void CheckBuildable()
        {
            if (!buildSocket.activeSelf)
                buildSocket.SetActive(true);

            float yRot = transform.eulerAngles.y;
            int nz = owner.currentGround.Pos.z;
            int nx = owner.currentGround.Pos.x;
            if ((yRot > 315f && yRot <= 360f) || (yRot > 0f && yRot <= 45f)) // +z ��
            {
                buildDirection = BuildDirection.Front;
                nz++;
            }
            else if (yRot > 45f && yRot <= 135f) // +x ��
            {
                buildDirection = BuildDirection.Right;
                nx++;
            }
            else if (yRot > 135f && yRot <= 225f)    // -z �� 
            {
                buildDirection = BuildDirection.Back;
                nz--;
            }
            else if (yRot > 225f && yRot <= 315f) // -x ��
            {
                buildDirection = BuildDirection.Left;
                nx--;
            }

            if (nz < 0 || nz > 59 || nx < 0 || nx > 59)
            {
                buildableGround = null;
                buildSocketRenderer.material = disableSocketMT;
                return;
            }

            // ���� ��ġ����
            buildSocket.transform.position = Manager.Navi.gameMap[nz].groundList[nx].transform.position;
            buildSocket.transform.position += Vector3.up;

            if (Manager.Navi.gameMap[nz].groundList[nx].type == GroundType.Buildable)
            {
                // ���� ��ġ�� ���� �� �ִ� ��ġ�� ��� �Ҵ�
                buildSocketRenderer.material = enableSocketMT;
                buildableGround = Manager.Navi.gameMap[nz].groundList[nx];
            }
            else
            {
                // �ƴҰ�� null �Ҵ�
                buildableGround = null;
                buildSocketRenderer.material = disableSocketMT;
            }
        }
    }
}
