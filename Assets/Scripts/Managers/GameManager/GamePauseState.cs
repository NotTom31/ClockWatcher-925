using UnityEngine;

public class GamePauseState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        //Toggle the pause UI to appear.
        UIManager.instance.TogglePauseUI();
        //unlocks the mouse and makes it visiable.
        CameraManager.instance.SetMouseLockState(false);

        //set time scale back to 0 to pause the game.
        Time.timeScale = 0f;

        //TODO Verify if we want to pause all audio or swap to menu music
        AudioListener.pause = true;
    }

    public override void OnExit(GameStateManager gameStateManager)
    {
        //Toggle the pause UI to disappear.
        UIManager.instance.TogglePauseUI();
        //Locks the mouse and makes it invisiable.
        CameraManager.instance.SetMouseLockState(true);
    }
}
