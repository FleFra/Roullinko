using UnityEngine;
using System.Collections.Generic;

public class PortalPegManager : MonoBehaviour
{
    [SerializeField] private int portalPegCount = 4;

    private List<PlinkoPeg> portalPegs = new List<PlinkoPeg>();

    private void Start()
    {
        PlinkoPeg[] allPegs = FindObjectsOfType<PlinkoPeg>();

        portalPegCount = Mathf.Min(portalPegCount, allPegs.Length);

        for (int i = 0; i < portalPegCount; i++)
        {
            int randomIndex = Random.Range(0, allPegs.Length);

            PlinkoPeg peg = allPegs[randomIndex];

            Renderer renderer = peg.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = Color.magenta;
            }

            portalPegs.Add(peg);

            allPegs[randomIndex] = allPegs[allPegs.Length - 1];
            System.Array.Resize(ref allPegs, allPegs.Length - 1);
        }

        Debug.Log($"Created {portalPegs.Count} portal pegs.");
    }

    public PlinkoPeg GetRandomPortal(PlinkoPeg currentPortal)
    {
        List<PlinkoPeg> availablePortals = new List<PlinkoPeg>();

        foreach (PlinkoPeg portal in portalPegs)
        {
            if (portal != currentPortal)
            {
                availablePortals.Add(portal);
            }
        }

        if (availablePortals.Count == 0)
            return null;

        return availablePortals[
            Random.Range(0, availablePortals.Count)
        ];
    }
}