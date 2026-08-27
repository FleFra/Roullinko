using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;      
    [SerializeField] private GameObject shopItemPrefab; 
    [SerializeField] private Transform contentParent;  

    private readonly List<ShopItemUI> spawnedItems = new List<ShopItemUI>();
    private bool listBuilt = false;

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged += HandleCurrencyChanged;

        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    public void OpenShop()
    {
        if (!listBuilt)
        {
            BuildShopList();
            listBuilt = true;
        }
        else
        {
            RefreshAllItems();
        }

        if (panelRoot != null)
            panelRoot.SetActive(true);
    }

    public void CloseShop()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);
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

    private void HandleCurrencyChanged(float newCurrency)
    {
        RefreshAllItems();
    }

    private void RefreshAllItems()
    {
        foreach (ShopItemUI item in spawnedItems)
        {
            item.Refresh();
        }
    }
}