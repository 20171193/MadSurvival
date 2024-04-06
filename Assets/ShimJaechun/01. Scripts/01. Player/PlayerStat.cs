using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public enum CoreStatType
    {
        NULL,
        Thirst,
        Hunger,
        HP,
        Stamina
    }

    public class PlayerStat : MonoBehaviour
    {
        [Header("�̵��ӵ�")]
        [SerializeField]
        private float maxSpeed;
        public float MaxSpeed { get { return maxSpeed; } }

        public float speedThreshold;

        [Header("ü��")]
        [SerializeField]
        private float maxHp;
        public float MaxHp { get { return maxHp; } }
        [SerializeField]
        private float ownHp;
        public float OwnHp{get { return ownHp; }
            set
            {
                ownHp = value;
                OnChangeHP?.Invoke(ownHp, maxHp);
            }
        }

        [Header("���׹̳�")]
        [SerializeField]
        private float maxStamina;
        public float MaxStamina { get { return maxStamina; } }

        [SerializeField]
        private float ownStamina;
        public float OwnStamina{get { return ownStamina; }
            set
            {
                ownStamina = value;
                OnChangeStamina?.Invoke(ownStamina, maxStamina);
            }
        }
        public float staminaDecValue;
        public float staminaIncValue;

        [Header("����")]
        [SerializeField]
        private float maxThirst;
        public float MaxThirst { get { return maxThirst; } }
        [SerializeField]
        private float ownThirst;
        public float OwnThirst { get { return ownThirst; } 
            set 
            { 
                ownThirst = value;
                OnChangeThirst?.Invoke(ownThirst, MaxThirst);
            }
        }

        [Header("���")]
        [SerializeField]
        private float maxHunger;
        public float MaxHunger { get { return maxHunger; } set { maxHunger = value; } }
        [SerializeField]
        private float ownHunger;
        public float OwnHunger { get { return ownHunger; } 
            set 
            {
                ownHunger = value;
                OnChangeHunger?.Invoke(ownHunger, maxHunger);
            }
        }

        [Header("���ݷ�")]
        [SerializeField]
        private float monsterAtk;  // ���ݷ�
        public float MonsterATK
        { get { return monsterAtk; } set { monsterAtk = value; } }

        [SerializeField]        // Ʈ�� ��� ���ݷ�
        private float treeAtk;
        public float TreeATK { get { return treeAtk; } set { treeAtk = value; } }

        [SerializeField]
        private float stoneAtk; // ���� ��� ���ݷ�
        public float StoneATK { get { return stoneAtk; } set { stoneAtk = value; } }

        [Header("���ݼӵ�")]
        [SerializeField]
        private float ats;  // ���ݼӵ�
        public float ATS
        {
            get
            {
                return ats;
            }
            set
            {
                ats = value;
            }
        }

        [Header("�˹�")]
        [SerializeField]
        private float knockbackPower;
        public float KnockbackPower { get { return knockbackPower; } }

        [SerializeField]
        private float knockbackTime;
        public float KnockbackTime { get { return knockbackTime; } }

        [Header("����")]
        [SerializeField]
        private float amr;  // ����
        public float AMR
        {
            get
            {
                return amr;
            }
            set
            {
                amr = value;
            }
        }

        [Header("�ǰ� �� �����ð�")]
        [SerializeField]
        private float invinsibleTime;   // �����ð� 
        public float InvinsibleTime { get { return invinsibleTime; } }

        [Header("UI ���� �̺�Ʈ")]
        public UnityEvent<float, float> OnChangeHunger;
        public UnityEvent<float, float> OnChangeThirst;
        public UnityEvent<float, float> OnChangeHP;
        public UnityEvent<float, float> OnChangeStamina;

        private Coroutine hpIncRoutine;
        private Coroutine hpDecRoutine;
        private Coroutine staminaIncRoutine;
        private Coroutine staminaDecRoutine;
        private Coroutine hungerIncRoutine;
        private Coroutine hungerDecRoutine;
        private Coroutine thirstIncRoutine;
        private Coroutine thirstDecRoutine;

        private Coroutine testRoutine;

        private void Awake()
        {
            OwnHp = maxHp;
            OwnStamina = maxStamina;
            OwnHunger = maxHunger;
            OwnThirst = maxThirst;
        }
        public void StartTimer(bool isIncrease, CoreStatType type, float value, float time = 1f, float maxTime = 0f)
        {
            switch (type)
            {
                case CoreStatType.Thirst:
                    if (isIncrease == true && thirstIncRoutine == null)
                        thirstIncRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    else if (isIncrease == false && thirstDecRoutine == null)
                        thirstDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    break;
                case CoreStatType.Hunger:
                    if (isIncrease == true && hungerIncRoutine == null)
                        hungerIncRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    else if (isIncrease == false && hungerDecRoutine == null)
                        hungerDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    break;
                case CoreStatType.HP:
                    if (isIncrease == true && hpIncRoutine == null)
                        hpIncRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    else if (isIncrease == false && hpDecRoutine == null)
                        hpDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    break;
                case CoreStatType.Stamina:
                    if (isIncrease == true && staminaIncRoutine == null)
                        staminaIncRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    else if (isIncrease == false && staminaDecRoutine == null)
                        staminaDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    break;
                default:
                    break;
            }
        }
        public void StopTimer(CoreStatType type, bool isIncrease)
        {
            switch (type)
            {
                case CoreStatType.Thirst:
                    if (isIncrease == true && thirstIncRoutine != null)
                        StopCoroutine(thirstIncRoutine);
                    else if (isIncrease == false && thirstDecRoutine != null)
                        StopCoroutine(thirstDecRoutine);
                    break;
                case CoreStatType.Hunger:
                    if (isIncrease == true && hungerIncRoutine != null)
                        StopCoroutine(hungerIncRoutine);
                    else if (isIncrease == false && hungerDecRoutine != null)
                        StopCoroutine(hungerDecRoutine);
                    break;
                case CoreStatType.HP:
                    if (isIncrease == true && hpIncRoutine != null)
                        StopCoroutine(hpIncRoutine);
                    else if (isIncrease == false && hpDecRoutine != null)
                        StopCoroutine(hpDecRoutine);
                    break;
                case CoreStatType.Stamina:
                    if (isIncrease == true && staminaIncRoutine != null)
                        StopCoroutine(staminaIncRoutine);
                    else if (isIncrease == false && staminaDecRoutine != null)
                        StopCoroutine(staminaDecRoutine);
                    break;
                default:
                    break;
            }
        }
        IEnumerator TimePerCoreValueRoutine(bool isIncrease, CoreStatType type, float value, float time = 1f, float maxTime = 0f)
        {
            bool infRun = maxTime == 0 ? true : false;
            bool flag = true;
            Coroutine curRoutine = null;
            float curTime = 0f;
            yield return null;
            while (flag && (infRun || curTime < maxTime))
            {
                switch (type)
                {
                    case CoreStatType.Thirst:
                        OwnThirst += isIncrease ? value : -value;
                        // ���� �����ִ� �ڷ�ƾ �Ҵ�
                        if (curRoutine == null)
                            curRoutine = isIncrease ? thirstIncRoutine : thirstDecRoutine;
                        // �ڷ�ƾ while�� ����
                        if ((ownThirst <= 0 && !isIncrease) || (ownThirst >= MaxThirst && isIncrease))
                            flag = false;
                        break;
                    case CoreStatType.Hunger:
                        OwnHunger += isIncrease ? value : -value;
                        if (curRoutine == null)
                            curRoutine = isIncrease ? hungerIncRoutine : hungerDecRoutine;
                        if ((OwnHunger <= 0 && !isIncrease) || (OwnHunger >= MaxHunger && isIncrease))
                            flag = false;
                        break;
                    case CoreStatType.HP:
                        OwnHp += isIncrease ? value : -value;
                        if (curRoutine == null)
                            curRoutine = isIncrease ? hpIncRoutine : hpDecRoutine;
                        if ((OwnHp <= 0 && !isIncrease) || (OwnHp >= MaxHp && isIncrease))
                            flag = false;
                        break;
                    case CoreStatType.Stamina:
                        OwnStamina += isIncrease ? value : -value;
                        if (curRoutine == null)
                            curRoutine = isIncrease ? staminaIncRoutine : staminaDecRoutine;
                        if ((OwnStamina <= 0 && !isIncrease) || (OwnStamina >= MaxStamina && isIncrease))
                            flag = false;
                        break;
                    default:
                        yield break;
                }
                curRoutine = null;
                curTime += time;
                yield return new WaitForSeconds(time);
            }
        }
    }
}
