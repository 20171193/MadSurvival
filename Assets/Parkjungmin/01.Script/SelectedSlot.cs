using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSlot : MonoBehaviour //pointclick �ڵ鷯�� ���õ� ����(�κ��丮)
{
    public static SelectedSlot instance;
    public Slot slot;

    private void Start()
    {
        instance = this;
    }

}
