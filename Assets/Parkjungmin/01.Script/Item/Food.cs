using Jc;
using jungmin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Food : Used_Item
{
    public Food(ItemData itemdata_) : base(itemdata_) { }

    [SerializeField] float IncreaseValue;
    public UnityAction OnUse;
    public override void Use(Player player)
    {
        player.Stat.OwnHunger += IncreaseValue;
        // 심재천 수정
        ScoreboardInvoker.Instance.eatMeat?.Invoke(ScoreType.Meat);
        OnUse();

    }
}
