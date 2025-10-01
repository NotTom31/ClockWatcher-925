using UnityEngine;

public class GameResumeState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        //set time scale back to 1 to resume the game.
        Time.timeScale = 1f;

        //TODO Verify if we want to pause all audio or swap to menu music
        AudioListener.pause = false;
    }
}
