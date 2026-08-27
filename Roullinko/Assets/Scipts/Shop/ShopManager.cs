using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private static ShopManager _instance;
    public static ShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<ShopManager>();
                if (_instance == null)
                {
                    Debug.LogError("[ShopManager] No ShopManager found in the scene at all! " +
                                   "Make sure a GameObject with ShopManager.cs exists and is active.");
                }
            }
            return _instance;
        }
    }

    [SerializeField] private List<UpgradeData> availableUpgrades = new List<UpgradeData>();
    [SerializeField] private PlinkoConfig plinkoConfig;

    public int PendingExtraGreenSpaces { get; private set; } = 0;

    public event Action<UpgradeData> OnUpgradePurchased;

    public IReadOnlyList<UpgradeData> AvailableUpgrades => availableUpgrades;

    private void Awake()
    {
        Debug.Log($"[ShopManager] Awake called on {gameObject.name}.");
        _instance = this;
    }

    public bool CanAfford(UpgradeData upgrade)
    {
        if (upgrade == null) return false;
        if (!upgrade.repeatable && upgrade.timesPurchased >= 1) return false;
        return GameManager.Instance.Currency >= upgrade.CurrentCost;
    }

    public bool TryPurchase(UpgradeData upgrade)
    {
        if (!CanAfford(upgrade)) return false;

        if (!GameManager.Instance.TrySpend(upgrade.CurrentCost)) return false;

        ApplyEffect(upgrade);
        upgrade.timesPurchased++;
        OnUpgradePurchased?.Invoke(upgrade);
        return true;
    }

    private void ApplyEffect(UpgradeData upgrade)
    {
        switch (upgrade.effectType)
        {
            case UpgradeEffectType.PlinkoMultiplierFlatBonus:
                plinkoConfig.AddFlatBonusToAllSlots(upgrade.value);
                break;

            case UpgradeEffectType.PlinkoMultiplierScale:
                plinkoConfig.ScaleAllSlots(upgrade.value);
                break;

            case UpgradeEffectType.RouletteExtraGreenSpace:
                PendingExtraGreenSpaces += Mathf.RoundToInt(upgrade.value);
                Debug.Log($"[ShopManager] Extra green space purchased — total pending: {PendingExtraGreenSpaces}. " +
                          "Apply this to the Roulette wheel config once it's built.");
                break;
        }
    }
}