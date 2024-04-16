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

        [Header("Á×ÀÎ ¸ó½ºÅÍ ¼ö")]
        public UnityEvent<ScoreType> killMonster;
        [Header("Á×ÀÎ µ¿¹° ¼ö")]
        public UnityEvent<ScoreType> killAnimal;
        [Header("¸ÔÀº °í±â °¹¼ö")]
        public UnityEvent<ScoreType> eatMeat;
        [Header("¹° ¸¶½Å È½¼ö")]
        public UnityEvent<ScoreType> drinkWater;
        [Header("Äµ ³ª¹«ÀÇ ¼ö")]
        public UnityEvent<ScoreType> digTree;
        [Header("Äµ µ¹ÀÇ ¼ö")]
        public UnityEvent<ScoreType> digStone;
    }
}
