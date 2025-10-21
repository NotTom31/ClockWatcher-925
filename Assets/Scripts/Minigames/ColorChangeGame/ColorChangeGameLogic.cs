using UnityEngine;

// Minigame description: A button changes color over time imperceptibly. After a certain amount of time, it reaches
// its "fail color" and the player's score begins dropping irrevocably. Pressing the button at any time resets the
// color and the timer.
public class ColorChangeGameLogic : MinigameLogic
{
    [SerializeField] ColorButton button;

    float score = 100f;
    public bool scoreDraining { get; set; }
    float scoreDrainRate = 2f;

    private void Update()
    {
        if (scoreDraining)
        {
            score -= scoreDrainRate * Time.deltaTime;
            if (score < 0f)
            {
                scoreDraining = false;
                score = 0f;
            }
        }
    }

    public override float EvaluateScore()
    {
        return score;
    }

    public override void InstantiateGame(int difficulty = 0)
    {
        button.changeRate = -0.01f;
        // TODO: based on difficulty, seed the button to change color at unpredictable rates
    }
}
