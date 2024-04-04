using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSlot_Recipe : MonoBehaviour
{
    public static SelectedSlot_Recipe instance;
    public RecipeSlot slot;
    void Start()
    {
        instance = this;
    }
}
