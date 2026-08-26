using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns one ShopItemUI per available upgrade under a scroll view content transform.
/// Attach to a "ShopUI" GameObject.
/// </summary>
public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject shopItemPrefab; // prefab with ShopItemUI on it
    [SerializeField] private Transform contentParent;   // the Content object of a Scroll View

    private readonly List<ShopItemUI> spawnedItems = new List<ShopItemUI>();

    private void Start()
    {
        BuildShopList();

        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    private void BuildShopList()
    {
        foreach (UpgradeData upgrade in ShopManager.Instance.AvailableUpgrades)
        {
            GameObject itemObj = Instantiate(shopItemPrefab, contentParent);
            ShopItemUI itemUI = itemObj.GetComponent<ShopItemUI>();
            itemUI.Setup(upgrade);
            spawnedItems.Add(itemUI);
        }
    }

    // Whenever currency changes, refresh every item's buy button (afford check may change).
    private void HandleCurrencyChanged(float newCurrency)
    {
        foreach (ShopItemUI item in spawnedItems)
        {
            item.Refresh();
        }
    }
}
