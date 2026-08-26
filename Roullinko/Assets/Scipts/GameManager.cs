using System;
using UnityEngine;

/// <summary>
/// Central hub for currency, daily play count, and the multiplier
/// handed off from Plinko to the next Roulette round.
/// Attach this to a single persistent GameObject (e.g. "GameManager") in your first scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Currency")]
    [SerializeField] private float currency = 100f;

    [Header("Daily Plays")]
    [SerializeField] private int playsRemainingToday = 5;

    // The multiplier Plinko produced, waiting to be applied to the Roulette bet.
    public float PendingMultiplier { get; private set; } = 1f;
    public bool HasPendingMultiplier { get; private set; } = false;

    public event Action<float> OnCurrencyChanged;
    public event Action<int> OnPlaysRemainingChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public float Currency => currency;
    public int PlaysRemainingToday => playsRemainingToday;

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

    public void ResetDailyPlays(int amount)
    {
        playsRemainingToday = amount;
        OnPlaysRemainingChanged?.Invoke(playsRemainingToday);
    }

    /// <summary>Called by PlinkoManager once a ball lands in a slot.</summary>
    public void SetPendingMultiplier(float multiplier)
    {
        PendingMultiplier = multiplier;
        HasPendingMultiplier = true;
    }

    /// <summary>Called by the Roulette flow once it has consumed the multiplier.</summary>
    public void ClearPendingMultiplier()
    {
        PendingMultiplier = 1f;
        HasPendingMultiplier = false;
    }
}
