using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class PlayerBaseState : BaseState
    {
        protected Player owner;
    }
    public class PlayerDiggable : PlayerBaseState
    {
        //public GameObject diggingImage;

        // ä��/������ ������ ����
        public PlayerDiggable(Player owner)
        {
            this.owner = owner;
        }
        //public override void Enter()
        //{
        //    diggingImage.SetActive(true);
        //}
        //public override void Exit()
        //{
        //    diggingImage.SetActive(false);
        //}
    }
    public class PlayerAttackable : PlayerBaseState
    {
        //public GameObject attackImage;

        // ������ ������ ����
        public PlayerAttackable(Player owner)
        {
            this.owner = owner;
        }

        //public override void Enter()
        //{
        //    attackImage.SetActive(true);
        //}
        //public override void Exit()
        //{
        //    attackImage.SetActive(false);
        //}
    }
    public class PlayerBuildable : PlayerBaseState
    {
        //public GameObject buildingImage;

        // �ǹ� ���Ⱑ ������ ����
        public PlayerBuildable(Player owner)
        {
            this.owner = owner;
        }

        //public override void Enter()
        //{
        //    buildingImage.SetActive(true);
        //}
        //public override void Exit()
        //{
        //    buildingImage.SetActive(false);
        //}
    }
}
