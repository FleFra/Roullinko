using UnityEngine;

/// <summary>
/// Data asset defining the multiplier for each Plinko slot.
/// Right-click in Project window -> Create -> Roullinko -> Plinko Config.
/// Kept as mutable runtime data (not constants) so upgrades like
/// "increase all Plinko multipliers by 1" can modify it directly.
/// </summary>
[CreateAssetMenu(fileName = "PlinkoConfig", menuName = "Roullinko/Plinko Config")]
public class PlinkoConfig : ScriptableObject
{
    [Tooltip("Multiplier value for each slot, left to right. Index 0 = leftmost slot.")]
    public float[] slotMultipliers = new float[]
    {
        5f, 2f, 1f, 0.5f, 0.2f, 0.5f, 1f, 2f, 5f
    };

    public int SlotCount => slotMultipliers.Length;

    public float GetMultiplier(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotMultipliers.Length)
        {
            Debug.LogWarning($"PlinkoConfig: slot index {slotIndex} out of range.");
            return 1f;
        }
        return slotMultipliers[slotIndex];
    }

    /// <summary>Upgrade hook: add a flat bonus to every slot's multiplier.</summary>
    public void AddFlatBonusToAllSlots(float bonus)
    {
        for (int i = 0; i < slotMultipliers.Length; i++)
        {
            slotMultipliers[i] += bonus;
        }
    }

    /// <summary>Upgrade hook: scale every slot's multiplier (e.g. 1.1 for +10%).</summary>
    public void ScaleAllSlots(float factor)
    {
        for (int i = 0; i < slotMultipliers.Length; i++)
        {
            slotMultipliers[i] *= factor;
        }
    }
}
