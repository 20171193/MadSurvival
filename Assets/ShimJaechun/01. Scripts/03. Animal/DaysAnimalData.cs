using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class DaysAnimalData : ScriptableObject
    {
        [Header("³¯Â¥ º° µ¿¹° µñ¼Å³Ê¸®")]
        public List<Dictionary<Animal, int>> daysAnimalList = new List<Dictionary<Animal,int>>();
    }
}
