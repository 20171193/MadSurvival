using Jc;
using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Food : Used_Item
{
    public Food(ItemData itemdata_) : base(itemdata_) { }

    [SerializeField] float IncreaseValue;

    public override void Use(Player player)
    {
        player.Stat.OwnHunger += IncreaseValue;
        // 심재천 수정
        ScoreboardInvoker.Instance.eatMeat?.Invoke(ScoreType.Meat);
    }
}
