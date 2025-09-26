using UnityEngine;

public class GameResumeState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        Debug.Log("Entered the Resume State.");

        Time.timeScale = 1f;

        //TODO Verify if we want to pause all audio or swap to menu music
        AudioListener.pause = false;
    }
}
