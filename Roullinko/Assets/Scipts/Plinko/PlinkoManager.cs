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

    public event Action<int, float> OnPlinkoResult;

    private bool ballInPlay = false;

    private void Awake()
    {
        Instance = this;
    }

    public bool PlaceBetAndDrop(float betAmount)
    {
        if (ballInPlay) return false;
        if (!GameManager.Instance.TryUsePlay()) return false;
        if (!GameManager.Instance.TrySpend(betAmount)) return false;

        // Remember the bet so the Roulette scene can apply the multiplier to it.
        PlayerPrefs.SetFloat("PendingBetAmount", betAmount); // swap for a proper data-passing system later
        DropBall();
        return true;
    }

    private void DropBall()
    {
        ballInPlay = true;
        float xOffset = UnityEngine.Random.Range(-spawnXRange, spawnXRange);
        Vector3 spawnPos = spawnPoint.position + new Vector3(xOffset, 0f, 0f);
        Instantiate(ballPrefab, spawnPos, Quaternion.identity);
    }

    public void OnBallLanded(int slotIndex, GameObject ballObject)
    {
        float multiplier = config.GetMultiplier(slotIndex);

        GameManager.Instance.SetPendingMultiplier(multiplier);
        OnPlinkoResult?.Invoke(slotIndex, multiplier);

        Destroy(ballObject);
        ballInPlay = false;
    }
}