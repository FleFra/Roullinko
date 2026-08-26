using UnityEngine;
using System.Collections.Generic;

public class BridgeBallManager : MonoBehaviour
{
    [SerializeField] private float bridgeRadius = 3f;
    [SerializeField] private GameObject bridgePrefab;

    private HashSet<GameObject> checkedPegs = new HashSet<GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject peg = collision.gameObject;

        if (!peg.CompareTag("Peg"))
            return;

        Renderer renderer = peg.GetComponent<Renderer>();

        if (renderer == null)
            return;

        // Check if the peg is green
        if (renderer.material.color != Color.green)
            return;

        Debug.Log("Ball hit green peg: " + peg.name);

        CheckPeg(peg);
    }

    private void CheckPeg(GameObject peg)
    {
        if (checkedPegs.Contains(peg))
            return;

        checkedPegs.Add(peg);

        GameObject[] allPegs = GameObject.FindGameObjectsWithTag("Peg");

        List<GameObject> nearbyGreenPegs = new List<GameObject>();

        foreach (GameObject otherPeg in allPegs)
        {
            if (otherPeg == peg)
                continue;

            Renderer renderer = otherPeg.GetComponent<Renderer>();

            if (renderer == null)
                continue;

            if (renderer.material.color != Color.green)
                continue;

            float distance = Vector2.Distance(
                peg.transform.position,
                otherPeg.transform.position
            );

            if (distance <= bridgeRadius)
            {
                nearbyGreenPegs.Add(otherPeg);
            }
        }

        Debug.Log($"Green pegs near {peg.name}: {nearbyGreenPegs.Count}");

        // No nearby green pegs
        if (nearbyGreenPegs.Count == 0)
        {
            Destroy(peg);
            return;
        }

        // Create bridges and continue the chain
        foreach (GameObject nearbyPeg in nearbyGreenPegs)
        {
            CreateBridge(peg, nearbyPeg);
            CheckPeg(nearbyPeg);
        }
    }

    private void CreateBridge(GameObject pegA, GameObject pegB)
    {
        Vector3 start = pegA.transform.position;
        Vector3 end = pegB.transform.position;

        Vector3 direction = end - start;
        float distance = direction.magnitude;

        GameObject bridge = Instantiate(
            bridgePrefab,
            (start + end) / 2f,
            Quaternion.identity
        );

        bridge.transform.localScale = new Vector3(
            distance,
            bridge.transform.localScale.y,
            bridge.transform.localScale.z
        );

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        bridge.transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angle
        );
    }
}