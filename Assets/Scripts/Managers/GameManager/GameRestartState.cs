using UnityEngine.SceneManagement;

public class GameRestartState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        //Reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
