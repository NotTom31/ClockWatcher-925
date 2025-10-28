using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        LevelManager.instance.DisableEnemies();
        LevelManager.instance.gameDifficulty++;

        if(LevelManager.instance.gameDifficulty >= 6)
        {
            gameStateManager.gameLoadLevelState.levelIndex = 2;
        }

        gameStateManager.SwitchState(gameStateManager.gameLoadLevelState);
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
 