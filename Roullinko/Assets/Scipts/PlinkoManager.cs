using System;
using UnityEngine;

/// <summary>
/// Drives the Plinko mini-game: takes the bet, drops the ball, and once it lands
/// in a slot, reads the multiplier from PlinkoConfig and hands it to GameManager
/// for use in the following Roulette round.
/// Attach to an empty "PlinkoManager" GameObject in the Plinko scene.
/// </summary>
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

    /// <summary>Fired when a ball lands: (slotIndex, multiplier).</summary>
    public event Action<int, float> OnPlinkoResult;

    private bool ballInPlay = false;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Call this from your bet UI. Deducts the bet, then drops a ball.
    /// This bet amount is the one that will actually apply to the *next* game (Roulette),
    /// per the Roullinko design — Plinko here is just deciding the multiplier.
    /// </summary>
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

    /// <summary>Called by PlinkoSlot when a ball triggers it.</summary>
    public void OnBallLanded(int slotIndex, GameObject ballObject)
    {
        float multiplier = config.GetMultiplier(slotIndex);

        GameManager.Instance.SetPendingMultiplier(multiplier);
        OnPlinkoResult?.Invoke(slotIndex, multiplier);

        Destroy(ballObject);
        ballInPlay = false;
    }
}
