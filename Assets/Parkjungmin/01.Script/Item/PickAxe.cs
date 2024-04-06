using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "PickAxe", menuName = "Item/EquipItem/PickAxe")]
    public class PickAxe : Equip_Item //���
    {
        [Header("��� ���ݷ�")]
        [SerializeField] float atk; //���ݷ�

        public override void Equip(Player player)
        {
            player.Stat.MonsterATK += atk;
        }

        public override void UnEquip(Player player)
        {
            player.Stat.MonsterATK -= atk;
        }
    }
}
