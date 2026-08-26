using UnityEngine;


public class CameraFollowBall : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private Vector3 defaultPosition;

    private void Start()
    {
        if (defaultPosition == Vector3.zero)
            defaultPosition = transform.position;
    }

    private void Update()
    {
        Transform currentBall = PlinkoManager.Instance != null ? PlinkoManager.Instance.CurrentBall : null;

        Vector3 targetPos = currentBall != null
            ? currentBall.position + offset
            : defaultPosition;

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}