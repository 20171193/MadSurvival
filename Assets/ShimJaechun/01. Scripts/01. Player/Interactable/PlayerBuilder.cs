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
        private GameObject buildSocket;             // 빌드소켓 오브젝트
        [SerializeField]
        private MeshRenderer buildSocketRenderer;   // 빌드소켓 오브젝트 렌더러

        [SerializeField]
        private Material enableSocketMT;    // 활성화 시 머터리얼
        [SerializeField]
        private Material disableSocketMT;   // 비활성화 시 머터리얼

        [SerializeField]
        private Ground buildableGround;
        [SerializeField]
        private BuildDirection buildDirection;
        [SerializeField]
        private Coroutine socketSetRoutine;

        private void Awake()
        {
            owner = GetComponent<Player>();
        }

        public void EnterBuildMode()
        {
            buildSocket.SetActive(true);
            if (socketSetRoutine == null)
                socketSetRoutine = StartCoroutine(SocketSetRoutine());
        }
        public void ExitBuildMode()
        {
            buildSocket.SetActive(false);
            if (socketSetRoutine != null)
            {
                StopCoroutine(socketSetRoutine);
                socketSetRoutine = null;
            }
        }

        public void Build(Build_Base buildItem)
        {
            if (buildableGround == null || buildItem == null) return;

            owner.ItemController.CurQuickSlot.itemCount--;
            if (owner.ItemController.CurQuickSlot.itemCount <= 0 )
            {
                owner.ItemController.CurQuickSlot.ClearSlot();
                owner.ItemController.ChangeButton();
            }

            buildItem.Build(buildableGround, buildDirection);
        }

        // 소켓 위치 지정
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
            if ((yRot > 315f && yRot <= 360f) || (yRot > 0f && yRot <= 45f)) // +z 앞
            {
                buildDirection = BuildDirection.Front;
                nz++;
            }
            else if (yRot > 45f && yRot <= 135f) // +x 우
            {
                buildDirection = BuildDirection.Right;
                nx++;
            }
            else if (yRot > 135f && yRot <= 225f)    // -z 뒤 
            {
                buildDirection = BuildDirection.Back;
                nz--;
            }
            else if (yRot > 225f && yRot <= 315f) // -x 좌
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

            // 소켓 위치지정
            buildSocket.transform.position = Manager.Navi.gameMap[nz].groundList[nx].transform.position;
            buildSocket.transform.position += Vector3.up;

            if (Manager.Navi.gameMap[nz].groundList[nx].type == GroundType.Buildable)
            {
                // 소켓 위치가 지을 수 있는 위치일 경우 할당
                buildSocketRenderer.material = enableSocketMT;
                buildableGround = Manager.Navi.gameMap[nz].groundList[nx];
            }
            else
            {
                // 아닐경우 null 할당
                buildableGround = null;
                buildSocketRenderer.material = disableSocketMT;
            }
        }
    }
}
