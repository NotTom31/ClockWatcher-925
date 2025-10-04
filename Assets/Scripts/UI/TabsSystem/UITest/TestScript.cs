using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestScript : MonoBehaviour
{
    public float startTime = 10f;         // How many seconds to count down from
    private float currentTime;
    public TextMeshProUGUI countdownText;            // Optional UI Text element

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;    // decrease time each frame
            currentTime = Mathf.Max(currentTime, 0); // clamp at zero

            // update UI text if assigned
            if (countdownText != null)
                countdownText.text = currentTime.ToString("0.0");
        }
    }
}
