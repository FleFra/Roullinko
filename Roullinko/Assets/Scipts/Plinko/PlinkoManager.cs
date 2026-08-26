using System;
using UnityEngine;

public class PlinkoManager : MonoBehaviour
{
    public static PlinkoManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlinkoConfig config;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn Settings")]
    [Tooltip("Horizontal range around spawnPoint the ball can start from.")]
    [SerializeField] private float spawnXRange = 0.2f;

    public event Action<int, float, float> OnPlinkoResult;

    private bool ballInPlay = false;
    private float currentBet;

    public Transform CurrentBall { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool PlaceBetAndDrop(float betAmount)
    {
        if (ballInPlay) return false;

        betAmount = Mathf.Clamp(betAmount, 1f, 10f);

        if (!GameManager.Instance.TryUsePlay()) return false;
        if (!GameManager.Instance.TrySpend(betAmount)) return false;

        currentBet = betAmount;
        DropBall();
        return true;
    }

    private void DropBall()
    {
        ballInPlay = true;
        float xOffset = UnityEngine.Random.Range(-spawnXRange, spawnXRange);
        Vector3 spawnPos = spawnPoint.position + new Vector3(xOffset, 0f, 0f);
        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        CurrentBall = ball.transform;
    }

    public void OnBallLanded(int slotIndex, GameObject ballObject)
    {
        float multiplier = config.GetMultiplier(slotIndex);
        float payout = currentBet * multiplier;

        Debug.Log($"Ball landed in slot {slotIndex} -> multiplier x{multiplier} -> bet {currentBet} pays out {payout}");

        GameManager.Instance.AddCurrency(payout);

        OnPlinkoResult?.Invoke(slotIndex, multiplier, payout);

        Destroy(ballObject);
        CurrentBall = null;
        ballInPlay = false;
    }
}