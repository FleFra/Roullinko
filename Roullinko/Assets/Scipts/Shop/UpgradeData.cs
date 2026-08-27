using UnityEngine;

public enum UpgradeEffectType
{
    PlinkoMultiplierFlatBonus,  // adds `value` to every Plinko slot's multiplier
    PlinkoMultiplierScale,      // multiplies every Plinko slot's multiplier by `value`
    RouletteExtraGreenSpace,    // placeholder until Roulette exists — currently just tracked, not applied
    IncreaseDailyPlays,         // permanently adds `value` (rounded) plays per day
}

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

    [HideInInspector] public int timesPurchased = 0;

    public float CurrentCost => cost + (timesPurchased * costIncreasePerPurchase);
}