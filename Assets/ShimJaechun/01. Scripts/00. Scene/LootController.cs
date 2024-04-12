using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Jc
{
    public class LootController : MonoBehaviour
    {
        [SerializeField]
        private Button[] button;

        public UnityEvent OnClickedLootButton;

        public void OnOpenLoot()
        {

        }

        public void OnClickLootButton()
        {
            OnClickedLootButton?.Invoke();
        }
    }
}
