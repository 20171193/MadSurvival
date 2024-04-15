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
        private PlayerStat playerStat;

        [SerializeField]
        private Button[] button;

        public UnityEvent OnClickedLootButton;

        public void OnOpenLoot()
        {

        }

        public void OnClickFirstButton()
        {
            Manager.Data.PrData.extraMonsterATK += 5f;
            OnClickedLootButton?.Invoke();

            playerStat.LoadBaseStat(Manager.Data.PrData);
        }
        public void OnClickSecondButton()
        {
            Manager.Data.PrData.extraTreeATK += 5f;
            Manager.Data.PrData.extraStoneATK += 5f;
            OnClickedLootButton?.Invoke();

            playerStat.LoadBaseStat(Manager.Data.PrData);
        }
        public void OnClickThirdButton()
        {
            Manager.Data.PrData.extraHunger += 10f;
            Manager.Data.PrData.extraThirst += 10f;
            OnClickedLootButton?.Invoke();

            playerStat.LoadBaseStat(Manager.Data.PrData);
        }
    }
}
