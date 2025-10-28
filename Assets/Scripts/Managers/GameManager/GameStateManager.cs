using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour, IDataPersistance
{
    GameBaseState currentState;

    public GamePauseState gamePauseState = new GamePauseState();
    public GameLoadLevelState gameLoadLevelState = new GameLoadLevelState();
    public GameRestartState gameRestartState = new GameRestartState();
    public GameResumeState gameResumeState = new GameResumeState();
    public GameStartState gameStartState = new GameStartState();
    public GameLoseState gameLoseState = new GameLoseState();
    public GameWinState gameWinState = new GameWinState();

    public static GameStateManager instance;

    private void Awake()
    {
        //Destroy second instance of GameStateManager if it exists.
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistance Manager. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
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

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SwitchState(gameRestartState);
    }

    /// <summary>
    /// Loads data from the current GameData File into the game
    /// </summary>
    /// <param name="data">Data from the file.</param>
    public void LoadData(GameData data)
    {
        //gameLoadLevelState.levelIndex = data.sceneToLoadIndex;

        ////Checks to see if the data file index matches the current scene, reload the level if not.
        //if (gameLoadLevelState.levelIndex != SceneManager.GetActiveScene().buildIndex)
        //{
        //    gameLoadLevelState.EnterState(this);
        //}
    }

    /// <summary>
    /// Saves data from the current game into the GameDataFile
    /// </summary>
    /// <param name="data">Data from the game.</param>
    public void SaveData(GameData data)
    {
        //data.sceneToLoadIndex = gameLoadLevelState.levelIndex;
    }
}
