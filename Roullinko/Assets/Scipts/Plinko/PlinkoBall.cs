using UnityEngine;

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
        float jitter = Random.Range(-spawnJitter, spawnJitter);
        rb.AddForce(new Vector2(jitter, 0f), ForceMode2D.Impulse);
    }
}
