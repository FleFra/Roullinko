using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Start()
    {
        Debug.Log($"[CurrencyDisplay] Start called. GameManager.Instance is {(GameManager.Instance == null ? "NULL" : "set")}. currencyText is {(currencyText == null ? "NULL" : "assigned")}.");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
            HandleCurrencyChanged(GameManager.Instance.Currency);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    private void HandleCurrencyChanged(float newAmount)
    {
        Debug.Log($"[CurrencyDisplay] HandleCurrencyChanged called with {newAmount}");
        currencyText.text = $"${newAmount:0.##}";
    }
}