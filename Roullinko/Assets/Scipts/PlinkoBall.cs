using UnityEngine;

/// <summary>
/// Attach to the Plinko ball prefab, alongside Rigidbody2D + CircleCollider2D.
/// Mostly a marker component; the actual physics is handled by Unity's engine
/// using the Rigidbody2D and a bouncy Physics Material 2D.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlinkoBall : MonoBehaviour
{
    [Tooltip("Small random horizontal nudge applied at spawn so identical drops don't always follow the same path.")]
    [SerializeField] private float spawnJitter = 0.05f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Tiny random push at spawn keeps outcomes from being perfectly repeatable,
        // which matters for a game that's supposed to feel fair and not scripted.
        float jitter = Random.Range(-spawnJitter, spawnJitter);
        rb.AddForce(new Vector2(jitter, 0f), ForceMode2D.Impulse);
    }
}
