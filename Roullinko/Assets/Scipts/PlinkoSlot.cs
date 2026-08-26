using UnityEngine;

public class PlinkoSlot : MonoBehaviour
{
    [SerializeField] private int slotIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlinkoBall ball = other.GetComponent<PlinkoBall>();
        if (ball == null) return;

        PlinkoManager.Instance.OnBallLanded(slotIndex, other.gameObject);
    }
}
