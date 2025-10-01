using UnityEngine;

public class UIManager : MonoBehaviour, IDataPersistance
{
    public GameObject pauseUI;

    public int interactionCount;

    private bool isEnabled;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        isEnabled = false;
    }

    /// <summary>
    /// Enables or disables the Pause UI for the player.
    /// </summary>
    public void TogglePauseUI()
    {
        //Toggles isEnabled flag.
        isEnabled = !isEnabled;

        //Set the pauseUI to active.
        pauseUI.SetActive(isEnabled);
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
