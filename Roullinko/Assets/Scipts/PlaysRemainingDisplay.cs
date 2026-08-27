using UnityEngine;
using TMPro;

public class PlaysRemainingDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playsText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlaysRemainingChanged += HandlePlaysChanged;
            HandlePlaysChanged(GameManager.Instance.PlaysRemainingToday);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlaysRemainingChanged -= HandlePlaysChanged;
    }

    private void HandlePlaysChanged(int playsRemaining)
    {
        playsText.text = $"Plays: {playsRemaining}";
    }
}
