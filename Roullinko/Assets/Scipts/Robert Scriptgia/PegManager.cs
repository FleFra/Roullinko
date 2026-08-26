using UnityEngine;

public class PegManager : MonoBehaviour
{
    [SerializeField] private int specialPegCount = 10;

    private void Start()
    {
        GameObject[] pegs = GameObject.FindGameObjectsWithTag("Peg");

        // Make sure we don't try to select more pegs than exist
        specialPegCount = Mathf.Min(specialPegCount, pegs.Length);

        // Pick random pegs
        for (int i = 0; i < specialPegCount; i++)
        {
            int randomIndex = Random.Range(0, pegs.Length);

            GameObject peg = pegs[randomIndex];

            Renderer renderer = peg.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }

            // Remove the selected peg from the pool
            pegs[randomIndex] = pegs[pegs.Length - 1];
            System.Array.Resize(ref pegs, pegs.Length - 1);
        }
    }
}