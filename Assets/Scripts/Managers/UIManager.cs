using UnityEngine;

public class UIManager : MonoBehaviour, IDataPersistance
{
    public GameObject pauseUI;
    public GameObject deathUI;

    public int interactionCount;

    private bool isPauseEnabled;

    private bool isDeathEnabled;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        isPauseEnabled = false;
    }

    /// <summary>
    /// Enables or disables the Pause UI for the player.
    /// </summary>
    public void TogglePauseUI()
    {
        //Toggles isEnabled flag.
        isPauseEnabled = !isPauseEnabled;

        //Set the pauseUI to active.
        pauseUI.SetActive(isPauseEnabled);
    }

    public void ToggleDeathUI()
    {
        //Toggles isEnabled flag.
        isDeathEnabled = !isDeathEnabled;

        //Set the DeathUI to active.
        deathUI.SetActive(isDeathEnabled);
    }

    public void LoadData(GameData data)
    {
        this.interactionCount = data.interactedCount;
    }
    public void SaveData(GameData data)
    {
        data.interactedCount = this.interactionCount;
    }
}
