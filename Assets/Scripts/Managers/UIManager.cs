using UnityEngine;

public class UIManager : MonoBehaviour, IDataPersistance
{
    private bool isEnabled;

    public static UIManager instance;

    public GameObject pauseUI;

    public int interactionCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        isEnabled = false;
    }
    public void TogglePauseUI()
    {
        isEnabled = !isEnabled;

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
