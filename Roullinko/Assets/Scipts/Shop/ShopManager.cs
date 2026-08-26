using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles purchasing upgrades and applying their effects.
/// Attach to an empty "ShopManager" GameObject.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private List<UpgradeData> availableUpgrades = new List<UpgradeData>();
    [SerializeField] private PlinkoConfig plinkoConfig;

    // Tracked here for now since Roulette doesn't exist yet — read this once you build
    // the Roulette wheel to know how many extra green spaces to add.
    public int PendingExtraGreenSpaces { get; private set; } = 0;

    /// <summary>Fired whenever an upgrade is successfully purchased.</summary>
    public event Action<UpgradeData> OnUpgradePurchased;

    public IReadOnlyList<UpgradeData> AvailableUpgrades => availableUpgrades;

    private void Awake()
    {
        Instance = this;
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
                // TODO: once Roulette exists, apply this directly to its wheel config instead.
                PendingExtraGreenSpaces += Mathf.RoundToInt(upgrade.value);
                Debug.Log($"[ShopManager] Extra green space purchased — total pending: {PendingExtraGreenSpaces}. " +
                          "Apply this to the Roulette wheel config once it's built.");
                break;
        }
    }
}
