using UnityEngine;

public class GameStartState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        CameraManager.instance.SetMouseLockState(true);
        PlayerManager.instance.onComputer = false;
        Time.timeScale = 1f;
    }
}
