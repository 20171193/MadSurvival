using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
        // �̱��� ���
        private static GroundSet inst;

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
            if (inst == null)
            {
                inst = this;

                // ��ã�� �Ŵ����� ���� ���� �Ҵ�
                if (groundLists.Count > 0)
                    Navi.AssginGameMap(groundLists);

                // �� ũ�� �Ҵ�
                Navi.mapZsize = zCount;
                Navi.mapXsize = xCount;

                // �÷��̾� ���� ��ǥ �Ҵ�
                Navi.cornerTL = new GroundPos(zCount / 3 - 1, xCount / 3 - 1);
                Navi.cornerTR = new GroundPos(zCount / 3 - 1, xCount / 3 * 2 - 1);
                Navi.cornerBL = new GroundPos(zCount / 3 * 2 - 1, xCount / 3 - 1);
                Navi.cornerBR = new GroundPos(zCount / 3 * 2 - 1, xCount / 3 * 2 - 1);

                waterSpawner.SettingGroundSize();

                DrawBuildableGround();
                waterSpawner.Spawn();
                CombineMeshes();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            GameFlowController.Inst.ExitNight();
        }

        // ����ȭ : �׶��� �޽� ����
        private void CombineMeshes()
        {
            // bfs Ž������ ���� ������ ���͸����� ����ϴ� �޽� ������ Ž���Ͽ� �ռ�
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };
            bool[,] visited = new bool[groundLists.Count, groundLists[0].groundList.Count];

            GameObject parentTr = new GameObject("Ground Combined Meshes");

            for (int i = 0; i < groundLists.Count; i++)
            {
                for (int j = 0; j < groundLists[i].groundList.Count; j++)
                {
                    // �湮�� ��ǥ�� ��� continue
                    if (visited[i, j]) continue;
                    // �޽����Ͱ� ���� ��� �湮üũ �� continue
                    if (groundLists[i].groundList[j].transform.GetChild(0).GetComponent<MeshFilter>() == null)
                    {
                        visited[i, j] = true;
                        continue;
                    }

                    // CombineMeshes�� ���� �Ѱ谡 ����.
                    // �ִ� 500������ ��ġ��.
                    int limitCount = 0;

                    // ������ �޽õ��� ���� ����Ʈ ����
                    List<CombineInstance> combineInstances = new List<CombineInstance>();

                    GameObject curModel = groundLists[i].groundList[j].transform.GetChild(0).gameObject;
                    Material curMaterial = curModel.GetComponent<MeshRenderer>().sharedMaterial;

                    CombineInstance combineInst = new CombineInstance();
                    // �޽� �Ҵ�
                    combineInst.mesh = curModel.GetComponent<MeshFilter>().sharedMesh;
                    // �޽� ��ȯ ���� �Ҵ�
                    combineInst.transform = curModel.transform.localToWorldMatrix;
                    combineInstances.Add(combineInst);
                    // �� ����
                    Destroy(curModel);
                    
                    Queue<GroundPos> q = new Queue<GroundPos>();
                    visited[i, j] = true;
                    q.Enqueue(new GroundPos(i, j));
                    limitCount++;
                    while (q.Count > 0)
                    {
                        GroundPos curPos = q.Dequeue();
                        for (int k = 0; k < 4; k++)
                        {
                            int nx = dx[k] + curPos.x;
                            int ny = dy[k] + curPos.z;
                            // Ž���� �� ������ ����ٸ� continue
                            if (nx < 0 || nx >= groundLists[0].groundList.Count || ny < 0 || ny >= groundLists.Count) continue;
                            // �̹� �湮�� ��� continue
                            if (visited[ny,nx]) continue;
                            // �޽����Ͱ� ���� ��� �湮üũ �� continue
                            if (groundLists[ny].groundList[nx].transform.GetChild(0).GetComponent<MeshFilter>() == null)
                            {
                                visited[ny, nx] = true;
                                continue;
                            }
                            // ���͸����� �ٸ� ��� continue
                            if (curMaterial != groundLists[ny].groundList[nx].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial) continue;

                            CombineInstance inst = new CombineInstance();
                            // Ground Object�� �޽� �� ����
                            GameObject meshModel = groundLists[ny].groundList[nx].transform.GetChild(0).gameObject;
                            // �޽����͸� ����ϴ� Top ������Ʈ�� �޽������� �����޽ø� �Ҵ�
                            MeshFilter meshFilter = meshModel.GetComponent<MeshFilter>();
                            // �޽� �Ҵ�
                            inst.mesh = meshFilter.sharedMesh;
                            // �޽� ��ȯ ���� �Ҵ�
                            inst.transform = meshModel.transform.localToWorldMatrix;
                            combineInstances.Add(inst);
                            // �� ����
                            Destroy(groundLists[ny].groundList[nx].transform.GetChild(0).gameObject);
                            
                            limitCount++;
                            q.Enqueue(new GroundPos(ny, nx));
                            visited[ny, nx] = true;
                        }
                        if (limitCount >= 10) break;
                    }

                    if (combineInstances.Count < 1) continue;

                    // �Ҵ�� Mesh���� Combine
                    Mesh combinedMesh = new Mesh();
                    combinedMesh.CombineMeshes(combineInstances.ToArray());
                    GameObject combinedModel = new GameObject("Combined Model");
                    combinedModel.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
                    combinedModel.AddComponent<MeshRenderer>().sharedMaterial = curMaterial;

                    // �׶��� ������Ʈ�� shadowcastingMode�� Off�� ����
                    combinedModel.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    combinedModel.transform.parent = parentTr.transform;
                }
            }
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
                    if (z == Navi.cornerTL.z + (Navi.cornerBL.z - Navi.cornerTL.z) / 2 &&
                        x == Navi.cornerTL.x + (Navi.cornerTR.x - Navi.cornerTL.x) / 2)
                    {
                        groundLists[z].groundList[x].type = GroundType.PlayerSpawn;
                        groundLists[z].groundList[x].OriginType = GroundType.PlayerSpawn;
                    }
                    else
                    {
                        groundLists[z].groundList[x].type = GroundType.Buildable;
                        groundLists[z].groundList[x].OriginType = GroundType.Buildable;
                    }
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
