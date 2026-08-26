using UnityEngine;

public class PlinkoTestButton : MonoBehaviour
{
    [SerializeField] private BetInputUI betInput;
    [SerializeField] private float fallbackBetAmount = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDropButtonPressed();
        }
    }

    public void OnDropButtonPressed()
    {
        float betAmount = betInput != null ? betInput.CurrentBetAmount : fallbackBetAmount;
        bool success = PlinkoManager.Instance.PlaceBetAndDrop(betAmount);
        if (!success)
        {
            Debug.Log("Drop failed — not enough currency, no plays left, or a ball is already in play.");
        }
    }
}