using UnityEngine;

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

    public void AddFlatBonusToAllSlots(float bonus)
    {
        for (int i = 0; i < slotMultipliers.Length; i++)
        {
            slotMultipliers[i] += bonus;
        }
    }

    public void ScaleAllSlots(float factor)
    {
        for (int i = 0; i < slotMultipliers.Length; i++)
        {
            slotMultipliers[i] *= factor;
        }
    }
}
