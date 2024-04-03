using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private ExplosionInvoker explosionInvoker;

        [SerializeField]
        private List<DropItem> dropItems;

        private void Awake()
        {
            if (dropItems.Count < 1) return;

            Manager.Pool.CreatePool(explosionInvoker, explosionInvoker.Size, explosionInvoker.Size + 5);
            foreach(DropItem item in dropItems)
            {
                Manager.Pool.CreatePool(item, item.Size, item.Size + 10);
            }
        }
    }
}
