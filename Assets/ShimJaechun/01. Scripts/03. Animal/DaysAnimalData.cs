using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class DaysAnimalData : ScriptableObject
    {
        [Header("��¥ �� ���� ��ųʸ�")]
        public List<Dictionary<Animal, int>> daysAnimalList = new List<Dictionary<Animal,int>>();
    }
}
