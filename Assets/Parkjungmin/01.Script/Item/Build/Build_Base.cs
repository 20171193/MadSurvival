using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jungmin;

namespace jungmin
{
    public class Build_Base : Item //�������� ���������� ������ �� ������
    {
        [SerializeField] GameObject bulidPrefab; //���� �������� �Ǽ� �� ������.
        public Build_Base(ItemData itemdata_) : base(itemdata_) { }
        [Header("�������� ü��")]
        [SerializeField] public float building_hp;
       // [SerializeField] public float building_Durable;


        public void Build()
        {
            
            GameObject building = Instantiate(bulidPrefab);
         
        }

    }
}
