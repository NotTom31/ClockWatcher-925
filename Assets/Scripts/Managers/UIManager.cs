using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IDataPersistance
{
    public GameObject pauseUI;
    public GameObject deathUI;
    public TextMeshProUGUI interactableText;

    public int interactionCount;

    private bool isPauseEnabled;
    private bool isDeathEnabled;
    private bool lookingAtInteractable;


    public static UIManager instance;

    private string interactableUIText = "E to ";

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

    public void ToggleInteractUI(bool lookingState)
    {
        //Toggles isEnabled flag.
        lookingAtInteractable = lookingState;

        //Set the DeathUI to active.
        interactableText.gameObject.SetActive(lookingAtInteractable);
    }

    public void SetUIText(string text = null)
    {
        if(!InputManager.instance.MenuFlag)
        {
            if (string.IsNullOrEmpty(text))
            {
                interactableText.text = interactableUIText + "Interact";
            }
            else
            {
                interactableText.text = interactableUIText + text;

            }
        }
        else
        {
            interactableText.text = "";
        }
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
