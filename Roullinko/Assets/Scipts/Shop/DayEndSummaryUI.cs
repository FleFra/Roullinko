using UnityEngine;
using TMPro;

/// <summary>
/// Shows an end-of-day summary panel with profit/loss when plays run out.
/// Attach to a UI Panel GameObject (start it inactive in the Inspector).
/// </summary>
public class DayEndSummaryUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;      // the panel to show/hide (can be this.gameObject)
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Color profitColor = Color.green;
    [SerializeField] private Color lossColor = Color.red;

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnDayEnded += HandleDayEnded;

        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnDayEnded -= HandleDayEnded;
    }

    private void HandleDayEnded(float profitLoss)
    {
        if (panelRoot != null)
            panelRoot.SetActive(true);

        bool isProfit = profitLoss >= 0f;
        string sign = isProfit ? "+" : "";
        resultText.text = isProfit
            ? $"Winst: {sign}${profitLoss:0.##}"
            : $"Verlies: -${Mathf.Abs(profitLoss):0.##}";
        resultText.color = isProfit ? profitColor : lossColor;
    }

    /// <summary>Call from a "Continue" button to close the panel and start a new day.</summary>
    public void OnContinuePressed(int newPlaysAmount)
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        GameManager.Instance.ResetDailyPlays(newPlaysAmount);
    }
}
