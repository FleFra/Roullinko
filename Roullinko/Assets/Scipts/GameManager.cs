using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Currency")]
    [SerializeField] private float currency = 100f;

    [Header("Daily Plays")]
    [SerializeField] private int playsRemainingToday = 5;

    private float currencyAtDayStart;

    public float PendingMultiplier { get; private set; } = 1f;
    public bool HasPendingMultiplier { get; private set; } = false;

    public event Action<float> OnCurrencyChanged;
    public event Action<int> OnPlaysRemainingChanged;

    public event Action<float> OnDayEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        currencyAtDayStart = currency;
    }

    public float Currency => currency;
    public int PlaysRemainingToday => playsRemainingToday;

    public float GetProfitLoss() => currency - currencyAtDayStart;

    public bool TrySpend(float amount)
    {
        if (amount <= 0f || amount > currency) return false;
        currency -= amount;
        OnCurrencyChanged?.Invoke(currency);
        return true;
    }

    public void AddCurrency(float amount)
    {
        if (amount <= 0f) return;
        currency += amount;
        OnCurrencyChanged?.Invoke(currency);
    }

    public bool TryUsePlay()
    {
        if (playsRemainingToday <= 0) return false;
        playsRemainingToday--;
        OnPlaysRemainingChanged?.Invoke(playsRemainingToday);
        return true;
    }

    public void NotifyRoundComplete()
    {
        if (playsRemainingToday == 0)
        {
            OnDayEnded?.Invoke(GetProfitLoss());
        }
    }

    public void ResetDailyPlays(int amount)
    {
        playsRemainingToday = amount;
        currencyAtDayStart = currency;
        OnPlaysRemainingChanged?.Invoke(playsRemainingToday);
    }

    public void SetPendingMultiplier(float multiplier)
    {
        PendingMultiplier = multiplier;
        HasPendingMultiplier = true;
    }

    public void ClearPendingMultiplier()
    {
        PendingMultiplier = 1f;
        HasPendingMultiplier = false;
    }
}