using System;
using System.Buffers;
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
        private Player owner;

        [Header("이동속도")]
        [SerializeField]
        private float maxSpeed;
        public float MaxSpeed { get { return maxSpeed; } }
        [SerializeField]
        private float curMaxSpeed;
        public float CurMaxSpeed { get { return curMaxSpeed; } }

        public float speedThreshold;

        [Header("체력")]
        [SerializeField]
        private float maxHp;
        public float MaxHp { get { return maxHp; } }
        [SerializeField]
        private float ownHp;
        public float OwnHp
        {
            get { return ownHp; }
            set
            {
                ownHp = value;
                OnChangeHP?.Invoke(ownHp, maxHp);
                if (ownHp <= 0f)
                    owner.OnDie();
            }
        }

        [SerializeField]
        private float hpDecValue;
        public float HpDecValue { get { return hpDecValue; } }

        [Header("스테미나")]
        [SerializeField]
        private float maxStamina;
        public float MaxStamina { get { return maxStamina; } }

        [SerializeField]
        private float ownStamina;
        public float OwnStamina
        {
            get { return ownStamina; }
            set
            {
                ownStamina = value;

                // 레버 이동범위 제한
                if (ownStamina / MaxStamina <= 0.01f)
                {
                    curMaxSpeed = maxSpeed / 2f;
                    EnterExhaust?.Invoke(); // 탈진 상태 시작
                }
                else if (curMaxSpeed < MaxSpeed && ownStamina / MaxStamina < 0.5f)
                {
                    curMaxSpeed = maxSpeed / 2f;
                }
                else
                {
                    ExitExhaust?.Invoke();  // 탈진 상태 탈출
                    curMaxSpeed = maxSpeed;
                }

                OnChangeStamina?.Invoke(ownStamina, maxStamina);
            }
        }

        public float staminaDecValue;
        public float staminaIncValue;

        [Header("갈증")]
        [SerializeField]
        private float maxThirst;
        public float MaxThirst { get { return maxThirst; } }

        [SerializeField]
        private float ownThirst;
        public float OwnThirst
        {
            get { return ownThirst; }
            set
            {              
                ownThirst = value;

                // 최대 수치 적용
                if (ownThirst > maxThirst)
                    ownThirst = maxThirst;

                // 갈증수치가 0인 경우  
                if (ownThirst <= 0f)
                {
                    ownThirst = 0f;
                    StartTimer(false, CoreStatType.HP, hpDecValue, 1f, 0f, CoreStatType.Thirst);
                }
                else
                    StopTimer(CoreStatType.HP, false, CoreStatType.Thirst);
                
                OnChangeThirst?.Invoke(ownThirst, MaxThirst);
            }
        }

        [Header("허기")]
        [SerializeField]
        private float maxHunger;
        public float MaxHunger { get { return maxHunger; } set { maxHunger = value; } }
        [SerializeField]
        private float ownHunger;
        public float OwnHunger
        {
            get { return ownHunger; }
            set
            {
                ownHunger = value;

                // 최대 수치 적용
                if (ownHunger > maxHunger)
                    ownHunger = maxHunger;

                // 허기수치가 0인 경우  
                if (ownHunger <= 0f)
                {
                    ownHunger = 0f;
                    StartTimer(false, CoreStatType.HP, hpDecValue, 1f, 0f, CoreStatType.Hunger);
                }
                else
                    StopTimer(CoreStatType.HP, false, CoreStatType.Hunger);

                OnChangeHunger?.Invoke(ownHunger, maxHunger);
            }
        }

        [Header("공격력")]
        [SerializeField]
        private float monsterAtk;  // 공격력
        public float MonsterATK
        { get { return monsterAtk; } set { monsterAtk = value; } }

        [SerializeField]        // 트리 대상 공격력
        private float treeAtk;
        public float TreeATK { get { return treeAtk; } set { treeAtk = value; } }

        [SerializeField]
        private float stoneAtk; // 바위 대상 공격력
        public float StoneATK { get { return stoneAtk; } set { stoneAtk = value; } }

        [Header("공격속도")]
        [SerializeField]
        private float ats;  // 공격속도
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

        [Header("넉백")]
        [SerializeField]
        private float knockbackPower;
        public float KnockbackPower { get { return knockbackPower; } }

        [SerializeField]
        private float knockbackTime;
        public float KnockbackTime { get { return knockbackTime; } }

        [Header("방어력")]
        [SerializeField]
        private float amr;  // 방어력
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

        [Header("피격 시 무적시간")]
        [SerializeField]
        private float invinsibleTime;   // 무적시간 
        public float InvinsibleTime { get { return invinsibleTime; } }

        [Header("UI 관련 이벤트")]
        public UnityEvent<float, float> OnChangeHunger;
        public UnityEvent<float, float> OnChangeThirst;
        public UnityEvent<float, float> OnChangeHP;
        public UnityEvent<float, float> OnChangeStamina;
        public UnityEvent EnterExhaust;
        public UnityEvent ExitExhaust;

        private Coroutine hpIncRoutine;
        private Coroutine hpHungerDecRoutine;
        private Coroutine hpThirstDecRoutine;
        private Coroutine staminaIncRoutine;
        private Coroutine staminaDecRoutine;
        private Coroutine hungerIncRoutine;
        private Coroutine hungerDecRoutine;
        private Coroutine thirstIncRoutine;
        private Coroutine thirstDecRoutine;

        private Coroutine testRoutine;

        private void Awake()
        {
            owner = GetComponent<Player>();
            curMaxSpeed = MaxSpeed;
            OwnHp = maxHp;
            OwnStamina = maxStamina;
            OwnHunger = maxHunger;
            OwnThirst = maxThirst;
        }
        // 스텟 증/감 타이머 시작
        public void StartTimer(bool isIncrease, CoreStatType type, float value, float time = 1f, float maxTime = 0f, CoreStatType causedType = CoreStatType.NULL)
        {
            switch (type)
            {
                case CoreStatType.Thirst:
                    if (isIncrease == true && thirstIncRoutine == null)
                        thirstIncRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    else if (isIncrease == false && thirstDecRoutine == null)
                        thirstDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, 4f, maxTime));
                    break;
                case CoreStatType.Hunger:
                    if (isIncrease == true && hungerIncRoutine == null)
                        hungerIncRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    else if (isIncrease == false && hungerDecRoutine == null)
                        hungerDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, 2f, maxTime));
                    break;
                case CoreStatType.HP:
                    if (isIncrease == true && hpIncRoutine == null)
                        hpIncRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime));
                    else if (isIncrease == false)
                    {
                        if (causedType == CoreStatType.Thirst && hpThirstDecRoutine == null)
                            hpThirstDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime, CoreStatType.Thirst));
                        else if (causedType == CoreStatType.Hunger && hpHungerDecRoutine == null)
                            hpHungerDecRoutine = StartCoroutine(TimePerCoreValueRoutine(isIncrease, type, value, time, maxTime, CoreStatType.Hunger));
                    }
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
        // 스텟 증/감 타이머 해제
        public void StopTimer(CoreStatType type, bool isIncrease, CoreStatType causedType = CoreStatType.NULL)
        {
            switch (type)
            {
                case CoreStatType.Thirst:
                    if (isIncrease == true && thirstIncRoutine != null)
                    {
                        StopCoroutine(thirstIncRoutine);
                        thirstIncRoutine = null;
                    }
                    else if (isIncrease == false && thirstDecRoutine != null)
                    {
                        StopCoroutine(thirstDecRoutine);
                        thirstDecRoutine = null;
                    }
                    break;
                case CoreStatType.Hunger:
                    if (isIncrease == true && hungerIncRoutine != null)
                    {
                        StopCoroutine(hungerIncRoutine);
                        hungerIncRoutine = null;
                    }
                    else if (isIncrease == false && hungerDecRoutine != null)
                    {
                        StopCoroutine(hungerDecRoutine);
                        hungerDecRoutine = null;
                    }
                    break;
                case CoreStatType.HP:
                    if (isIncrease == true && hpIncRoutine != null)
                    {
                        StopCoroutine(hpIncRoutine);
                        hpIncRoutine = null;
                    }
                    else if (isIncrease == false)
                    {
                        if (causedType == CoreStatType.Thirst && hpThirstDecRoutine != null)
                        {
                            StopCoroutine(hpThirstDecRoutine);
                            hpThirstDecRoutine = null;
                        }
                        else if (causedType == CoreStatType.Hunger && hpHungerDecRoutine != null)
                        {
                            StopCoroutine(hpHungerDecRoutine);
                            hpHungerDecRoutine = null;
                        }
                    }
                    break;
                case CoreStatType.Stamina:
                    if (isIncrease == true && staminaIncRoutine != null)
                    {
                        StopCoroutine(staminaIncRoutine);
                        staminaIncRoutine = null;
                    }
                    else if (isIncrease == false && staminaDecRoutine != null)
                    {
                        StopCoroutine(staminaDecRoutine);
                        staminaIncRoutine = null;
                    }
                    break;
                default:
                    break;
            }
        }
        IEnumerator TimePerCoreValueRoutine(bool isIncrease, CoreStatType type, float value, float time = 1f, float maxTime = 0f, CoreStatType causedType = CoreStatType.NULL)
        {
            // 무한 루프를 사용할 것인지?
            bool infRun = (maxTime == 0f ? true : false);
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
                        // 현재 돌고있는 코루틴 할당
                        if (curRoutine == null)
                            curRoutine = isIncrease ? thirstIncRoutine : thirstDecRoutine;
                        // 코루틴 while문 종료
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
                            curRoutine = isIncrease ? hpIncRoutine : (causedType == CoreStatType.Thirst ? hpThirstDecRoutine : hpHungerDecRoutine);
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
