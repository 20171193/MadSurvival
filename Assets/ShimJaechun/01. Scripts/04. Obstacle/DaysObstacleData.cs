using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class DaysObstacleData : ScriptableObject
    {
        // ��¥�� ��ֹ� ���� ������
        [Header("������ ���� ����Ʈ (�ε��� - ����)")]
        public List<int> trees = new List<int>(3) { 0,0,0};

        [Header("������ �� ����Ʈ (�ε��� - ����)")]
        public List<int> stones = new List<int>(3) { 0,0,0};
    }
}
