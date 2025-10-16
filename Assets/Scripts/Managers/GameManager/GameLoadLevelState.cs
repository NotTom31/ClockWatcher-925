using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadLevelState : GameBaseState
{
    public int levelIndex;
    public GameLoadLevelState() : base()
    {
        //Setting GameLoadLevel index to 0 for the main menu. (Make sure to set the project setting)
        levelIndex = 0;
    }

    public GameLoadLevelState(int levelIndex) : base()
    {
        this.levelIndex = levelIndex;
    }

    public override void EnterState(GameStateManager gameStateManager)
    {
        //Saving to the data file before leaving the level.
        DataPersistenceManager.instance.SaveGame();

        //Goes to next project build scene
        SceneManager.LoadSceneAsync(levelIndex);

    }
}
