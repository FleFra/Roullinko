using UnityEngine;
using TMPro;

public class DayEndSummaryUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
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

    public void OnContinuePressed()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        GameManager.Instance.ResetDailyPlays();
    }
}