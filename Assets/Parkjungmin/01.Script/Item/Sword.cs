using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jungmin
{
    [CreateAssetMenu(fileName = "Sword", menuName = "Item/EquipItem/Sword")]
    public class Sword : Equip_Item
    {
        [Header("�� ���ݷ�")]
        [SerializeField] float atk; //���ݷ�

        public override void Equip(Player player)
        {
            player.Stat.MonsterATK += atk;
            Debug.Log($"{atk}�� �ö󰬽��ϴ�.");
        }

        public override void UnEquip(Player player)
        {
            player.Stat.MonsterATK -= atk;
        }
    }
}
