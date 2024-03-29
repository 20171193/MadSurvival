using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jc
{
    [Serializable]
    public struct GroundList
    {
        [SerializeField]
        public List<Ground> groundList;

        public GroundList(List<Ground> groundList)
        {
            this.groundList = groundList;
        }
    }

    public class GroundSet : MonoBehaviour
    {
        [Header("Component")]
        [Space(2)]
        [SerializeField]
        private GameObject groundPrefab;

        [SerializeField]
        private MeshRenderer dummyPlaneMr;

        [SerializeField]
        private Material buildableMt;
        [SerializeField]
        private Material waterMt;

        //[SerializeField]
        //private List<List<Ground>> groundList = new List<List<Ground>>();

        [SerializeField]
        private List<GroundList> groundLists;
        public List<GroundList> GroundLists { get { return groundLists; } }

        // 프리팹 크기
        [SerializeField]
        private float prefabXscale;
        [SerializeField]
        private float prefabZscale;

        // x축에 생성할 프리팹 개수 
        private int xCount;
        // z축에 생성할 프리팹 개수
        private int zCount;

        // x 축 끝점 (첫 프리팹이 생성될 x좌표)
        private float xStartPos;
        // z 축 끝점 (첫 프리팹이 생성될 z좌표)
        private float zStartPos;

        private void Start()
        {
            // 길찾기 매니저에 게임 맵을 할당
            //if (groundLists.Count > 0)
            //Manager.Navigation.AssginGameMap(groundLists);
        }


        [ContextMenu("DrawGround")]
        public void DrawGround()
        {
            groundLists.Clear();

            dummyPlaneMr.enabled = false;

            prefabXscale = groundPrefab.transform.localScale.x;
            prefabZscale = groundPrefab.transform.localScale.z;

            // 추후 로직 수정 (계산식 오류)
            // 프리팹 스케일이 1 또는 2일때만 정상적으로 동작
            xStartPos = 4.5f * transform.localScale.x - 0.5f * (prefabXscale - 1);
            zStartPos = 4.5f * transform.localScale.z - 0.5f * (prefabZscale - 1);

            xCount = (int)(transform.localScale.x * 10 / prefabXscale);
            zCount = (int)(transform.localScale.z * 10 / prefabZscale);

            // 맵 크기 할당
            Manager.Navi.mapZsize = zCount;
            Manager.Navi.mapXsize = xCount;

            // 플레이어 진지 좌표 할당
            Manager.Navi.cornerTL = new GroundPos(zCount / 3 - 1, xCount / 3 - 1);
            Manager.Navi.cornerTR = new GroundPos(zCount / 3 * 2 - 1, xCount / 3 - 1);
            Manager.Navi.cornerBL = new GroundPos(zCount / 3 - 1, xCount / 3 * 2 - 1);
            Manager.Navi.cornerBR = new GroundPos(zCount / 3 * 2 - 1, xCount / 3 * 2 - 1);

            for (int z = 0; z < zCount; z++)
            {
                GroundList grounds = new GroundList(new List<Ground>());

                for (int x = 0; x < xCount; x++)
                {
                    Vector3 groundPos = new Vector3(
                        transform.position.x - xStartPos + x * prefabXscale,
                        transform.position.y,
                        transform.position.z - zStartPos + z * prefabZscale);
                    GameObject inst = Instantiate(groundPrefab, groundPos, Quaternion.identity);
                    grounds.groundList.Add(inst.GetComponent<Ground>());
                    inst.transform.parent = transform;
                    
                    // Buildable Type 세팅
                    if(x >= Manager.Navi.cornerTL.x && x <= Manager.Navi.cornerTR.x &&
                        z <= Manager.Navi.cornerBL.z && z >= Manager.Navi.cornerTL.z )
                    {
                        inst.transform.GetChild(0).GetComponent<MeshRenderer>().materials[0] = buildableMt;
                    }

                    inst.gameObject.name = $"{z},{x}";
                }

                groundLists.Add(grounds);
            }
        }

        [ContextMenu("ResetGround")]
        public void ResetGround()
        {
            groundLists.Clear();
        }
    }
}
