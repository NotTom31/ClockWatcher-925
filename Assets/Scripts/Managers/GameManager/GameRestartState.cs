using UnityEngine.SceneManagement;

public class GameRestartState : GameBaseState
{   public override void EnterState(GameStateManager gameStateManager)
    {
        //Reloads the scene
        //StartCoroutine("StartDeathScene");
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }
    public override void UpdateState(GameStateManager gameStateManager)
    {
        //Reloads the scene
    }
}
