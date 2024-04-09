using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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
        private WaterSet waterSpawner;
        [SerializeField]
        private ObstacleSpawner obstacleSpawner;
        [SerializeField]
        private GameObject groundPrefab;
        [SerializeField]
        private MeshRenderer dummyPlaneMr;
        [SerializeField]
        private List<Material> buildableMt;
        [SerializeField]
        private Material waterMt;

        //[SerializeField]
        //private List<List<Ground>> groundList = new List<List<Ground>>();

        [SerializeField]
        private List<GroundList> groundLists;
        public List<GroundList> GroundLists { get { return groundLists; } }

        // ������ ũ��
        [SerializeField]
        private float prefabXscale;
        [SerializeField]
        private float prefabZscale;

        // x�࿡ ������ ������ ���� 
        [SerializeField]
        private int xCount;
        // z�࿡ ������ ������ ����
        [SerializeField]
        private int zCount;

        // x �� ���� (ù �������� ������ x��ǥ)
        private float xStartPos;
        // z �� ���� (ù �������� ������ z��ǥ)
        private float zStartPos;

        // ����ȭ
        private NavigationManager Navi => Manager.Navi;

        private void Start()
        {
            // ��ã�� �Ŵ����� ���� ���� �Ҵ�
            if (groundLists.Count > 0)
                Navi.AssginGameMap(groundLists);

            // �� ũ�� �Ҵ�
            Navi.mapZsize = zCount;
            Navi.mapXsize = xCount;

            // �÷��̾� ���� ��ǥ �Ҵ�
            Navi.cornerTL = new GroundPos(zCount / 3 - 1, xCount / 3 - 1);
            Navi.cornerTR = new GroundPos(zCount / 3 - 1, xCount / 3*2 - 1);
            Navi.cornerBL = new GroundPos(zCount / 3 * 2 - 1, xCount / 3 - 1);
            Navi.cornerBR = new GroundPos(zCount / 3 * 2 - 1, xCount / 3 * 2 - 1);

            waterSpawner.SettingGroundSize();

            DrawBuildableGround();
            waterSpawner.Spawn();
            GameFlowController.Inst.ExitNight();
        }


        [ContextMenu("DrawGround")]
        public void DrawGround()
        {
            groundLists.Clear();

            dummyPlaneMr.enabled = false;

            prefabXscale = groundPrefab.transform.localScale.x;
            prefabZscale = groundPrefab.transform.localScale.z;

            // ���� ���� ���� (���� ����)
            // ������ �������� 1 �Ǵ� 2�϶��� ���������� ����
            xStartPos = 4.5f * transform.localScale.x - 0.5f * (prefabXscale - 1);
            zStartPos = 4.5f * transform.localScale.z - 0.5f * (prefabZscale - 1);

            xCount = (int)(transform.localScale.x * 10 / prefabXscale);
            zCount = (int)(transform.localScale.z * 10 / prefabZscale);

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
                    Ground groundInst = inst.GetComponent<Ground>();
                    groundInst.OriginType = GroundType.Empty;
                    grounds.groundList.Add(groundInst);
                    inst.transform.parent = transform;
                    inst.gameObject.name = $"{z},{x}";
                }

                groundLists.Add(grounds);
            }
        }

        public void DrawBuildableGround()
        {
            for (int z = Navi.cornerTL.z; z <= Navi.cornerBL.z; z++)
            {
                for (int x = Navi.cornerTL.x; x <= Navi.cornerTR.x; x++)
                {
                    if(z == Navi.cornerTL.z + (Navi.cornerBL.z - Navi.cornerTL.z)/2 &&
                        x == Navi.cornerTL.x + (Navi.cornerTR.x - Navi.cornerTL.x) / 2)
                    {
                        groundLists[z].groundList[x].type = GroundType.PlayerSpawn;
                        groundLists[z].groundList[x].OriginType = GroundType.PlayerSpawn;
                        continue;
                    }
                    groundLists[z].groundList[x].type = GroundType.Buildable;
                    groundLists[z].groundList[x].OriginType = GroundType.Buildable;
                    groundLists[z].groundList[x].transform.GetChild(0).GetComponent<MeshRenderer>().SetMaterials(buildableMt);
                }
            }
        }

        [ContextMenu("ResetGround")]
        public void ResetGround()
        {
            groundLists.Clear();
        }
    }
}
