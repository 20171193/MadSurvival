using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace jungmin
{
    public class CraftItem : MonoBehaviour
    {
        public GameData gamedata;

        void Save()
        {
            string json = JsonUtility.ToJson(gamedata);
            Debug.Log(gamedata);
        }
    }

    [Serializable]
    public class GameData
    {
        public int Tear;
        public int goid;
        public int exp;
    }
}