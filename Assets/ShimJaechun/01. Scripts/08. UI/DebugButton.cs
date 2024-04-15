using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class DebugButton : MonoBehaviour
    {
        [SerializeField]
        private DebugController debuger;
        
        [SerializeField]
        private PlayerStatType statType;

        public void OnClickUpButton()
        {
            debuger.OnClickUpButton(statType);
        }
        public void OnClickDownButton()
        {
            debuger.OnClickDownButton(statType);
        }
    }
}
