using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
