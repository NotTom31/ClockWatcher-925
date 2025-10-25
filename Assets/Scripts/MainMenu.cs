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

        DataPersistenceManager.instance.NewGame();

        SceneManager.LoadScene(1);
    }

    public void OnContinuedGameCliked()
    {
        DisableMenuButtons();

        DataPersistenceManager.instance.LoadGame();
    }

    public void OnQuitGameCliked()
    {
        Application.Quit();
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
    }
}
