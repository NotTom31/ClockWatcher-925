using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadLevelState : GameBaseState
{
    public int levelIndex;
    public GameLoadLevelState() : base()
    {
        levelIndex = 0;
    }
    public GameLoadLevelState(int levelIndex) : base()
    {
        this.levelIndex = levelIndex;
    }
    public override void EnterState(GameStateManager gameStateManager)
    {
        Debug.Log("Entered the Progress State.");

        DataPersistenceManager.instance.SaveGame();

        //Goes to next project build scene
        SceneManager.LoadSceneAsync(levelIndex);

    }
}
