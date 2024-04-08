using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : MonoBehaviour
{
    public item itemData;
    public bool isTouched = false;

    void Start()
    {
        gameObject.name = this.itemData.name;
    }
}
