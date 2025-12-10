using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public void Resume()
    {
        GameStateManager.instance.SwitchState(GameStateManager.instance.gameResumeState);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
