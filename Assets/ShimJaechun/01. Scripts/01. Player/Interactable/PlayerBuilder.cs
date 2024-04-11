using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;
namespace Jc
{
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

            buildItem.Build();
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
            Debug.Log(yRot);
            int nz = owner.currentGround.Pos.z;
            int nx = owner.currentGround.Pos.x;
            if ((yRot > 315f && yRot <= 360f) || (yRot > 0f && yRot <= 45f)) // +z ��
                nz++;
            else if (yRot > 45f && yRot <= 135f) // +x ��
                nx++;
            else if (yRot > 135f && yRot <= 225f)    // -z �� 
                nz--;
            else if (yRot > 225f && yRot <= 315f) // -x ��
                nx--;

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
