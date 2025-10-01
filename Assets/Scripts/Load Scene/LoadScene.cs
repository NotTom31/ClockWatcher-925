using UnityEngine;
using UnityEngine.SceneManagement;

//TODO Impelment a LoadScene class and Manager to set up loading screnes between scenes.
public class LoadScene : MonoBehaviour
{
    public static LoadScene instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistance Manager. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}