using UnityEngine;

/// <summary>
/// The type of effect an upgrade applies. Add new types here as you build more mechanics
/// (e.g. RouletteExtraGreen once the Roulette wheel config exists).
/// </summary>
public enum UpgradeEffectType
{
    PlinkoMultiplierFlatBonus,  // adds `value` to every Plinko slot's multiplier
    PlinkoMultiplierScale,      // multiplies every Plinko slot's multiplier by `value`
    RouletteExtraGreenSpace,    // placeholder until Roulette exists — currently just tracked, not applied
}

/// <summary>
/// Data asset for a single shop upgrade.
/// Right-click in Project window -> Create -> Roullinko -> Upgrade Data.
/// </summary>
[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Roullinko/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName = "New Upgrade";
    [TextArea] public string description = "What this upgrade does.";
    public float cost = 50f;
    public UpgradeEffectType effectType;
    [Tooltip("Meaning depends on effectType — e.g. how much to add or scale by.")]
    public float value = 1f;

    [Tooltip("If true, cost increases each time it's purchased (see costIncreasePerPurchase). If false, it's a one-time buy.")]
    public bool repeatable = true;
    public float costIncreasePerPurchase = 25f;

    // Runtime-only, not saved to the asset file — tracks purchases during play.
    [HideInInspector] public int timesPurchased = 0;

    public float CurrentCost => cost + (timesPurchased * costIncreasePerPurchase);
}
