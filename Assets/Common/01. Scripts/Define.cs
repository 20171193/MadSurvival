using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public static class Define
    {
        public enum MonsterName
        {
            Skeleton = 0,
            GreenSkeleton,
            RedSkeleton,
            BlackSkeleton,
            BigSkeleton,
            GreenBigSkeleton,
            RedBigSkeleton,
            BlackBigSkeleton,
            Golem,
            BlackGolem
        }

        public static Dictionary<MonsterName, string> enumToNameDic;
        public static string TryGetMonsterName(MonsterName enumName)
        {
            if(enumToNameDic.Count < 1)
            {
                enumToNameDic = new Dictionary<MonsterName, string>();
                // ��ųʸ��� ����ٸ� ����
                for(int i = 0; i<10; i++)
                {
                    enumToNameDic.Add((MonsterName)i, ((MonsterName)i).ToString());    
                }
            }

            return enumToNameDic[enumName];
        }
    }
}
