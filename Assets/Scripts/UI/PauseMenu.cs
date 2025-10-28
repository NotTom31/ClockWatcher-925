using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        GameStateManager.instance.SwitchState(GameStateManager.instance.gameResumeState);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
