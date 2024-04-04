using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class DaysObstacleData : ScriptableObject
    {
        // 날짜별 장애물 스폰 데이터
        [Header("스폰할 나무 리스트 (인덱스 - 레벨)")]
        public List<int> trees = new List<int>(3) { 0,0,0};

        [Header("스폰할 돌 리스트 (인덱스 - 레벨)")]
        public List<int> stones = new List<int>(3) { 0,0,0};
    }
}
