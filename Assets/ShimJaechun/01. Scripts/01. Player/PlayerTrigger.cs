using Jc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using jungmin;
namespace Jc
{
    // �浹���� �̺�Ʈó��
    public class PlayerTrigger : MonoBehaviour, IDamageable
    {
        public Player owner;

        public void TakeDamage(float value)
        {
            float damage = value - owner.Stat.AMR;
            if (damage < 1) return;

            // ���� ���� ���
            if (owner.ItemController.CurArmorItem != null)
            {
                owner.ItemController.CurArmorItem.Durable--;

                // �� �������� ��� �Ҹ�� ���
                if(owner.ItemController.CurArmorItem.Durable < 1)
                    owner.ItemController.UnEquip(Equip_Item.EquipType.Armor);
            }
            owner.Stat.OwnHp -= damage;
            if (owner.Stat.OwnHp > 0)
            {
                owner.Anim.SetTrigger("OnHit");
                owner.OnTakeDamage();
            }
        }
        public void GetItem(Item item)
        {
            owner.GetItem(item);
        }


        private void OnTriggerStay(Collider other)
        {
            // ���� �����ϰ� �ִ� ���
            if(Manager.Layer.waterLM.Contain(other.gameObject.layer))
            {
                owner.IsOnWater = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // ������ ��� ���
            if (Manager.Layer.waterLM.Contain(other.gameObject.layer))
            {
                owner.IsOnWater = false;
            }
        }
    }
}
