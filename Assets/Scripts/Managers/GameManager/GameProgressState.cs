using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressState : GameBaseState
{
    public override void EnterState(GameStateManager gameStateManager)
    {
        Debug.Log("Entered the Progress State.");

        //Goes to next project build scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
