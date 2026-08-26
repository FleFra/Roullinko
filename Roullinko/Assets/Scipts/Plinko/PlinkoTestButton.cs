using UnityEngine;

public class PlinkoTestButton : MonoBehaviour
{
    [SerializeField] private float testBetAmount = 10f;

    public void OnDropButtonPressed()
    {
        bool success = PlinkoManager.Instance.PlaceBetAndDrop(testBetAmount);
        if (!success)
        {
            Debug.Log("Drop failed — not enough currency, no plays left, or a ball is already in play.");
        }
    }
}
