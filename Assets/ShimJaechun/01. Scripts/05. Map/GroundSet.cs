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
        // 싱글턴 사용
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

        // 프리팹 크기
        [SerializeField]
        private float prefabXscale;
        [SerializeField]
        private float prefabZscale;

        // x축에 생성할 프리팹 개수 
        [SerializeField]
        private int xCount;
        // z축에 생성할 프리팹 개수
        [SerializeField]
        private int zCount;

        // x 축 끝점 (첫 프리팹이 생성될 x좌표)
        private float xStartPos;
        // z 축 끝점 (첫 프리팹이 생성될 z좌표)
        private float zStartPos;

        // 간략화
        private NavigationManager Navi => Manager.Navi;

        private void Start()
        {
            if (inst == null)
            {
                inst = this;

                // 길찾기 매니저에 게임 맵을 할당
                if (groundLists.Count > 0)
                    Navi.AssginGameMap(groundLists);

                // 맵 크기 할당
                Navi.mapZsize = zCount;
                Navi.mapXsize = xCount;

                // 플레이어 진지 좌표 할당
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

        // 최적화 : 그라운드 메시 결합
        private void CombineMeshes()
        {
            // bfs 탐색으로 같은 종류의 머터리얼을 사용하는 메시 집합을 탐색하여 합성
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };
            bool[,] visited = new bool[groundLists.Count, groundLists[0].groundList.Count];

            GameObject parentTr = new GameObject("Ground Combined Meshes");

            for (int i = 0; i < groundLists.Count; i++)
            {
                for (int j = 0; j < groundLists[i].groundList.Count; j++)
                {
                    // 방문한 좌표일 경우 continue
                    if (visited[i, j]) continue;
                    // 메시필터가 없는 경우 방문체크 후 continue
                    if (groundLists[i].groundList[j].transform.GetChild(0).GetComponent<MeshFilter>() == null)
                    {
                        visited[i, j] = true;
                        continue;
                    }

                    // CombineMeshes의 정점 한계가 있음.
                    // 최대 500개까지 합치기.
                    int limitCount = 0;

                    // 결합할 메시들을 담을 리스트 생성
                    List<CombineInstance> combineInstances = new List<CombineInstance>();

                    GameObject curModel = groundLists[i].groundList[j].transform.GetChild(0).gameObject;
                    Material curMaterial = curModel.GetComponent<MeshRenderer>().sharedMaterial;

                    CombineInstance combineInst = new CombineInstance();
                    // 메시 할당
                    combineInst.mesh = curModel.GetComponent<MeshFilter>().sharedMesh;
                    // 메시 변환 정보 할당
                    combineInst.transform = curModel.transform.localToWorldMatrix;
                    combineInstances.Add(combineInst);
                    // 모델 삭제
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
                            // 탐색할 맵 범위를 벗어났다면 continue
                            if (nx < 0 || nx >= groundLists[0].groundList.Count || ny < 0 || ny >= groundLists.Count) continue;
                            // 이미 방문한 경우 continue
                            if (visited[ny,nx]) continue;
                            // 메시필터가 없는 경우 방문체크 후 continue
                            if (groundLists[ny].groundList[nx].transform.GetChild(0).GetComponent<MeshFilter>() == null)
                            {
                                visited[ny, nx] = true;
                                continue;
                            }
                            // 머터리얼이 다른 경우 continue
                            if (curMaterial != groundLists[ny].groundList[nx].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial) continue;

                            CombineInstance inst = new CombineInstance();
                            // Ground Object의 메시 모델 추출
                            GameObject meshModel = groundLists[ny].groundList[nx].transform.GetChild(0).gameObject;
                            // 메시필터를 사용하는 Top 오브젝트의 메시필터의 공유메시를 할당
                            MeshFilter meshFilter = meshModel.GetComponent<MeshFilter>();
                            // 메시 할당
                            inst.mesh = meshFilter.sharedMesh;
                            // 메시 변환 정보 할당
                            inst.transform = meshModel.transform.localToWorldMatrix;
                            combineInstances.Add(inst);
                            // 모델 삭제
                            Destroy(groundLists[ny].groundList[nx].transform.GetChild(0).gameObject);
                            
                            limitCount++;
                            q.Enqueue(new GroundPos(ny, nx));
                            visited[ny, nx] = true;
                        }
                        if (limitCount >= 10) break;
                    }

                    if (combineInstances.Count < 1) continue;

                    // 할당된 Mesh들을 Combine
                    Mesh combinedMesh = new Mesh();
                    combinedMesh.CombineMeshes(combineInstances.ToArray());
                    GameObject combinedModel = new GameObject("Combined Model");
                    combinedModel.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
                    combinedModel.AddComponent<MeshRenderer>().sharedMaterial = curMaterial;

                    // 그라운드 오브젝트의 shadowcastingMode를 Off로 변경
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

            // 추후 로직 수정 (계산식 오류)
            // 프리팹 스케일이 1 또는 2일때만 정상적으로 동작
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
