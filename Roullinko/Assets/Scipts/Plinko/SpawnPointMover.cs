using UnityEngine;

public class SpawnPointMover : MonoBehaviour
{
    [SerializeField] private float moveRange = 2f;   
    [SerializeField] private float speed = 2f;        

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float xOffset = Mathf.Sin(Time.time * speed) * moveRange;
        transform.position = startPos + new Vector3(xOffset, 0f, 0f);
    }
}
