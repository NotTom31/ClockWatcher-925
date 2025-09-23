using UnityEngine;

public class GamePauseState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        Debug.Log("Entered the Pause State.");

        Time.timeScale = 0f;

        //TODO Verify if we want to pause all audio or swap to menu music
        AudioListener.pause = true;
    }

}
