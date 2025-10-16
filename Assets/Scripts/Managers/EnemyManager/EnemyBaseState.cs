public class EnemyBaseState
{
    /// <summary>
    /// Handles any logic that needs to be done when entering the state.
    /// </summary>
    /// <param name="enemyStateManager">The Game State Manager</param>
    public virtual void EnterState(EnemyStateManager enemyStateManager)
    {

    }

    /// <summary>
    /// Handles any logic that needs to be done every update frame.
    /// </summary>
    /// <param name="enemyStateManager">The Game State Manager</param>
    public virtual void UpdateState(EnemyStateManager enemyStateManager)
    {

    }

    /// <summary>
    /// Handles any logic that needs to be done before leaving the state
    /// </summary>
    /// <param name="enemyStateManager">The Game State Manager</param>
    public virtual void OnExit(EnemyStateManager enemyStateManager)
    {

    }
}
