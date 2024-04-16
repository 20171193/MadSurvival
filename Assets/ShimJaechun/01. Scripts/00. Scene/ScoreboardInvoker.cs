using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class ScoreboardInvoker : MonoBehaviour
    {
        private static ScoreboardInvoker inst;
        public static ScoreboardInvoker Instance { get { return inst; } }

        private void Awake()
        {
            inst = this;
        }

        [Header("���� ���� ��")]
        public UnityEvent<ScoreType> killMonster;
        [Header("���� ���� ��")]
        public UnityEvent<ScoreType> killAnimal;
        [Header("���� ��� ����")]
        public UnityEvent<ScoreType> eatMeat;
        [Header("�� ���� Ƚ��")]
        public UnityEvent<ScoreType> drinkWater;
        [Header("ĵ ������ ��")]
        public UnityEvent<ScoreType> digTree;
        [Header("ĵ ���� ��")]
        public UnityEvent<ScoreType> digStone;
    }
}
