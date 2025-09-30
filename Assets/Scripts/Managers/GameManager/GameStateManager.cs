using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    GameBaseState currentState;

    public GamePauseState gamePauseState = new GamePauseState();
    public GameLoadLevelState gameLoadLevelState = new GameLoadLevelState();
    public GameRestartState gameRestartState = new GameRestartState();
    public GameResumeState gameResumeState = new GameResumeState();
    public GameStartState gameStartState = new GameStartState();

    public void Start()
    {
        currentState = gameStartState;

        currentState.EnterState(this);
    }

    
    public void Update()
    {
        currentState.UpdateState(this);
    }

    /// <summary>
    /// Handles exiting the current state the game is in and switches to the new state.
    /// </summary>
    /// <param name="state">The new state for the system to be on.</param>
    public void SwitchState(GameBaseState state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        currentState.EnterState(this);    
    }
}
