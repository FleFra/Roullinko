using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls a single upgrade row/card in the shop.
/// Attach to a prefab containing: a name text, a description text, a cost text, and a Buy button.
/// </summary>
public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    private UpgradeData upgradeData;

    public void Setup(UpgradeData data)
    {
        upgradeData = data;
        nameText.text = data.upgradeName;
        descriptionText.text = data.description;

        buyButton.onClick.AddListener(HandleBuyClicked);

        Refresh();
    }

    private void HandleBuyClicked()
    {
        if (ShopManager.Instance.TryPurchase(upgradeData))
        {
            Refresh();
        }
    }

    /// <summary>Call after any currency change to keep the cost/button state accurate.</summary>
    public void Refresh()
    {
        costText.text = $"${upgradeData.CurrentCost:0.##}";

        bool soldOut = !upgradeData.repeatable && upgradeData.timesPurchased >= 1;
        bool affordable = ShopManager.Instance.CanAfford(upgradeData);

        buyButton.interactable = affordable && !soldOut;
        costText.text = soldOut ? "Sold" : $"${upgradeData.CurrentCost:0.##}";
    }
}
