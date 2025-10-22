using UnityEngine;

public class GameBaseState
{
    /// <summary>
    /// Handles any logic that needs to be done when entering the state.
    /// </summary>
    /// <param name="gameStateManager">The Game State Manager</param>
    public virtual void EnterState(GameStateManager gameStateManager)
    {

    }

    /// <summary>
    /// Handles any logic that needs to be done every update frame.
    /// </summary>
    /// <param name="gameStateManager">The Game State Manager</param>
    public virtual void UpdateState(GameStateManager gameStateManager)
    { 

    }

    /// <summary>
    /// Handles any logic that needs to be done before leaving the state
    /// </summary>
    /// <param name="gameStateManager">The Game State Manager</param>
    public virtual void OnExit(GameStateManager gameStateManager)
    {

    }
}
