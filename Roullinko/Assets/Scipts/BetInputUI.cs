using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Bet input UI: a Slider (min 1, max 10, whole numbers) plus a label showing
/// the current value. Reads the slider's value when a drop is triggered.
/// Attach to a UI GameObject holding a Slider and a TextMeshProUGUI label.
/// </summary>
public class BetInputUI : MonoBehaviour
{
    [SerializeField] private Slider betSlider;
    [SerializeField] private TextMeshProUGUI betValueText;

    private void Start()
    {
        if (betSlider != null)
        {
            betSlider.minValue = 1;
            betSlider.maxValue = 10;
            betSlider.wholeNumbers = true;
            betSlider.value = 1;
            betSlider.onValueChanged.AddListener(HandleSliderChanged);
            HandleSliderChanged(betSlider.value);
        }
    }

    private void HandleSliderChanged(float value)
    {
        if (betValueText != null)
            betValueText.text = $"Bet: {value:0}";
    }

    /// <summary>Current bet amount, read by whatever triggers the drop (button or Space key).</summary>
    public float CurrentBetAmount => betSlider != null ? betSlider.value : 1f;
}
