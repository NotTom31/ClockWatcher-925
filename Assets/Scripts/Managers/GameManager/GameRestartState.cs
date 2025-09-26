using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestartState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        Debug.Log("Entered the Restart State.");
        
        //Reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
