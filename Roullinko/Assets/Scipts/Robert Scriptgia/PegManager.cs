using UnityEngine;
using System.Collections.Generic;

public class PegManager : MonoBehaviour
{
    [Header("Starting Pegs")]
    [SerializeField] private int startingPegCount = 10;

    [Header("Generation")]
    [SerializeField] private float generationRadius = 3f;
    [SerializeField][Range(0f, 100f)] private float generationChance = 35f;

    private List<PlinkoPeg> allPegs = new List<PlinkoPeg>();
    private List<PlinkoPeg> greenPegs = new List<PlinkoPeg>();
    private HashSet<PlinkoPeg> checkedPegs = new HashSet<PlinkoPeg>();

    private void Start()
    {
        FindPegs();
        CreateStartingPegs();
        GenerateGreenPegs();
    }

    private void FindPegs()
    {
        PlinkoPeg[] pegs = FindObjectsOfType<PlinkoPeg>();

        allPegs.AddRange(pegs);

        Debug.Log($"Found {allPegs.Count} pegs.");
    }

    private void CreateStartingPegs()
    {
        int amount = Mathf.Min(startingPegCount, allPegs.Count);

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, allPegs.Count);

            PlinkoPeg peg = allPegs[randomIndex];

            MakeGreen(peg);

            // Remove from available starting pegs
            allPegs.RemoveAt(randomIndex);
        }
    }

    private void GenerateGreenPegs()
    {
        int currentIndex = 0;

        while (currentIndex < greenPegs.Count)
        {
            PlinkoPeg currentPeg = greenPegs[currentIndex];

            if (!checkedPegs.Contains(currentPeg))
            {
                CheckForNearbyPegs(currentPeg);
                checkedPegs.Add(currentPeg);
            }

            currentIndex++;
        }

        Debug.Log($"Generated {greenPegs.Count} green pegs.");
    }

    private void CheckForNearbyPegs(PlinkoPeg currentPeg)
    {
        foreach (PlinkoPeg otherPeg in allPegs)
        {
            if (otherPeg == null)
                continue;

            float distance = Vector2.Distance(
                currentPeg.transform.position,
                otherPeg.transform.position
            );

            if (distance > generationRadius)
                continue;

            // Roll the chance
            float roll = Random.Range(0f, 100f);

            if (roll <= generationChance)
            {
                MakeGreen(otherPeg);
            }
        }
    }

    private void MakeGreen(PlinkoPeg peg)
    {
        if (greenPegs.Contains(peg))
            return;

        Renderer renderer = peg.GetComponent<Renderer>();

        if (renderer == null)
            return;

        renderer.material.color = Color.green;

        greenPegs.Add(peg);
    }
}