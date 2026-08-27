using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgeBallManager : MonoBehaviour
{
    [Header("Green Peg Bridges")]
    [SerializeField] private float bridgeRadius = 3f;
    [SerializeField] private GameObject bridgePrefab;

    private PortalPegManager portalPegManager;

    private HashSet<GameObject> checkedPegs = new HashSet<GameObject>();

    private bool portalCooldown = false;

    private void Start()
    {
        portalPegManager = FindObjectOfType<PortalPegManager>();

        if (portalPegManager == null)
        {
            Debug.LogWarning("No PortalPegManager found in the scene!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject peg = collision.gameObject;

        // Make sure this is a Plinko peg
        PlinkoPeg plinkoPeg = peg.GetComponent<PlinkoPeg>();

        if (plinkoPeg == null)
            return;

        Renderer renderer = peg.GetComponent<Renderer>();

        if (renderer == null)
            return;

        // =========================
        // PURPLE PORTAL PEG
        // =========================

        if (renderer.material.color == Color.magenta)
        {
            if (!portalCooldown)
            {
                TeleportToRandomPortal(peg);
            }

            return;
        }

        // =========================
        // GREEN SPECIAL PEG
        // =========================

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

        PlinkoPeg[] allPegs = FindObjectsOfType<PlinkoPeg>();

        List<GameObject> nearbyGreenPegs = new List<GameObject>();

        foreach (PlinkoPeg otherPegComponent in allPegs)
        {
            GameObject otherPeg = otherPegComponent.gameObject;

            if (otherPeg == peg)
                continue;

            Renderer renderer = otherPeg.GetComponent<Renderer>();

            if (renderer == null)
                continue;

            if (renderer.material.color != Color.green)
                continue;

            // Only connect to pegs below the current peg
            if (otherPeg.transform.position.y >= peg.transform.position.y)
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

        Debug.Log(
            $"Green pegs below {peg.name} within radius: {nearbyGreenPegs.Count}"
        );

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

    private void TeleportToRandomPortal(GameObject currentPeg)
    {
        if (portalPegManager == null)
        {
            Debug.LogWarning("No PortalPegManager found!");
            return;
        }

        PlinkoPeg currentPortal = currentPeg.GetComponent<PlinkoPeg>();

        if (currentPortal == null)
            return;

        PlinkoPeg destination = portalPegManager.GetRandomPortal(
            currentPortal
        );

        if (destination == null)
            return;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 currentVelocity = Vector2.zero;

        if (rb != null)
        {
            currentVelocity = rb.linearVelocity;
        }

        // Teleport
        transform.position = destination.transform.position;

        // Keep velocity
        if (rb != null)
        {
            rb.linearVelocity = currentVelocity;
        }

        StartCoroutine(PortalCooldown());

        Debug.Log(
            $"Teleported from {currentPortal.name} to {destination.name}"
        );
    }

    private IEnumerator PortalCooldown()
    {
        portalCooldown = true;

        yield return new WaitForSeconds(0.15f);

        portalCooldown = false;
    }
}