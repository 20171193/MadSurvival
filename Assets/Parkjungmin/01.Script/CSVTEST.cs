using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace jungmin
{
    public class CSVTEST : MonoBehaviour
    {
        void Start()
        {
            List<Dictionary<string, object>> reader = CSVReader.Read("CSV1/ItemCSV");
            Debug.Log((string)reader[0]["name"]);
        }
    }
}

