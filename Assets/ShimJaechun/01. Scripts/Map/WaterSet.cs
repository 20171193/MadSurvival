using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class WaterSet : MonoBehaviour
    {
        [Serializable]
        public struct WaterGround
        {
            public GroundPos startPos;
            public int zSize;
            public int xSize;
            public WaterGround(GroundPos startPos, int zSize, int xSize)
            {
                this.startPos = startPos;
                this.zSize = zSize;
                this.xSize = xSize;
            }
        }

        [SerializeField]
        private GameObject waterPrefab;

        [SerializeField]
        private int waterSize;
        [SerializeField]
        private bool isRandomSpawn;
        [SerializeField]
        private int spawnSize;

        [Header("8방향 스폰위치 {0~3:모서리, 4:윗변, 5:아랫변, 6:좌측변, 7:우측변}")]
        [SerializeField]
        private List<WaterGround> groundList;

        private void Awake()
        {
            groundList = new List<WaterGround>();
        }

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            // 랜덤으로 spawnSize 만큼만 뽑아서 스폰
            if(isRandomSpawn)
            {
                List<int> idxList = new List<int>();
                for(int i = 0; i< groundList.Count; i++)
                {
                    idxList.Add(i);
                }

                int cnt = spawnSize;
                while (cnt > 0)
                {
                    int rand = UnityEngine.Random.Range(0, idxList.Count - 1);
                    SpawnWaterGround(idxList[rand]);
                    idxList.RemoveAt(rand);
                    cnt--;
                }
            }
            else
            {
                for(int i =0; i<groundList.Count; i++)
                {
                    SpawnWaterGround(i);
                }
            }
        }

        public void SpawnWaterGround(int index)
        {
            if (index < 0 || index >= groundList.Count)
            {
                Debug.Log("SpawnWaterGround : 인덱스 범위 벗어남");
                return;
            }

            WaterGround waterGround = groundList[index];
            for (int z = waterGround.startPos.z; z < waterGround.startPos.z + waterGround.zSize; z++)
            {
                for (int x = waterGround.startPos.x; x < waterGround.startPos.x + waterGround.xSize; x++)
                {
                    Ground ground = Manager.Navi.gameMap[z].groundList[x];
                    Destroy(ground.transform.GetChild(0).gameObject);
                    GameObject water = Instantiate(waterPrefab, ground.transform.position + Vector3.up, Quaternion.identity);
                    water.transform.parent = transform;
                    water.name = $"{z},{x}_Water";
                    ground.type = GroundType.Water;
                }
            }
        }
        public void SettingGroundSize()
        {
            // 모서리 세팅
            int zSize = 8, xSize = 8;
            // 1. 좌상단
            groundList.Add(new WaterGround(new GroundPos(0, 0), zSize, xSize));
            // 2. 우상단
            groundList.Add(new WaterGround(new GroundPos(0, Manager.Navi.mapXsize - xSize), zSize, xSize));
            // 3. 좌하단
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize - zSize, 0), zSize, xSize));
            // 4. 우하단
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize - zSize, Manager.Navi.mapXsize - xSize), zSize, xSize));

            // 중상/중하단 세팅
            zSize = 5;
            xSize = 13;
            // 1. 중상단
            groundList.Add(new WaterGround(new GroundPos(0, Manager.Navi.mapXsize / 2 - xSize / 2), zSize, xSize));
            // 2. 중하단
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize - zSize, Manager.Navi.mapXsize / 2 - xSize / 2), zSize, xSize));

            // 좌중/우중단 세팅
            zSize = 13;
            xSize = 5;
            // 1. 좌중단
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize / 2 - zSize / 2, 0), zSize, xSize));
            // 2. 우중단
            groundList.Add(new WaterGround(new GroundPos(Manager.Navi.mapZsize / 2 - zSize / 2, Manager.Navi.mapXsize - xSize), zSize, xSize));
        }
    }
}
