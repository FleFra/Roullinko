using UnityEngine;
using TMPro;

public class SpeedToggle : MonoBehaviour
{
    [SerializeField] private float fastSpeed = 2f;
    [SerializeField] private TextMeshProUGUI buttonLabel;

    private bool isFast = false;

    public void ToggleSpeed()
    {
        isFast = !isFast;
        Time.timeScale = isFast ? fastSpeed : 1f;

        if (buttonLabel != null)
            buttonLabel.text = isFast ? "1x" : "2x";
    }
}
