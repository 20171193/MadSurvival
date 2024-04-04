using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Recipe
{

    public string name;
    [SerializeField] public IGD IGD_1; //�� �������� ���� ��� 1
    [SerializeField] public IGD IGD_2; //�� �������� ���� ��� 2
    [SerializeField] public IGD IGD_3; //�� �������� ���� ��� 3


}
[Serializable]
public struct IGD // �����ǿ��� �������� ���
{
    public string IGD_Name; //����� �̸�
    public int IGD_Count; //����� ����
}
