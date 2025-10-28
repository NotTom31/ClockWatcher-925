using UnityEngine;

public class GameLoseState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        Debug.Log("Entered Lose State.");
        //Make the Mouse Vistable
        CameraManager.instance.SetMouseLockState(false);

        //Disable most inputs
        InputManager.instance.MenuFlag = true;

        //set time scale back to 0 to pause the game.
        Time.timeScale = 0f;

        //TODO Verify if we want to pause all audio or swap to menu music
        AudioListener.pause = true;
    }

    /// <summary>
    /// Handles any logic that needs to be done every update frame.
    /// </summary>
    /// <param name="gameStateManager">The Game State Manager</param>
    public override void UpdateState(GameStateManager gameStateManager)
    {

    }

    /// <summary>
    /// Handles any logic that needs to be done before leaving the state
    /// </summary>
    /// <param name="gameStateManager">The Game State Manager</param>
    public override void OnExit(GameStateManager gameStateManager)
    {

    }

}
 