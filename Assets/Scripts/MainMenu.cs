using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    public void OnNewGameClicked()
    {
        DisableMenuButtons();

        Debug.Log("New Game Clicked");

        DataPersistenceManager.instance.NewGame();

        SceneManager.LoadSceneAsync("SetupScene");
    }

    public void OnContinuedGameCliked()
    {
        DisableMenuButtons();

        Debug.Log("Continue Clicked");
        DataPersistenceManager.instance.LoadGame();

    }

    public void OnQuitGameCliked()
    {
        Debug.Log("Quit Application Clicked");
        //Application.Quit();
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
    }
}
