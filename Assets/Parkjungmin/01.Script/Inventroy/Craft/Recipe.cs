using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Recipe
{

    public string name;
    [SerializeField] public IGD IGD_1; //이 아이템을 만들 재료 1
    [SerializeField] public IGD IGD_2; //이 아이템을 만들 재료 2
    [SerializeField] public IGD IGD_3; //이 아이템을 만들 재료 3


}
[Serializable]
public struct IGD // 레시피에서 아이템의 재료
{
    public string IGD_Name; //재료의 이름
    public int IGD_Count; //재료의 개수
}
