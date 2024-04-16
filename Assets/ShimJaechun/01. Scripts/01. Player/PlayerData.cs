using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float prevMonsterATK;
    public float prevTreeATK;
    public float prevStoneATK;
    public float prevHunger;
    public float prevThirst;

    public float extraMonsterATK;
    public float extraTreeATK;
    public float extraStoneATK;
    public float extraHunger;
    public float extraThirst;

    public PlayerData()
    {
        prevMonsterATK = 0f;
        prevTreeATK = 0f;
        prevStoneATK = 0f;
        prevHunger = 0f;
        prevThirst = 0f;

        extraMonsterATK = 0f;
        extraTreeATK = 0f;
        extraHunger = 0f;

        extraHunger = 0f;
        extraThirst = 0f;
    }
}
