using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestartState : GameBaseState
{    public override void EnterState(GameStateManager gameStateManager)
    {
        //Reloads the scene
        //StartCoroutine("StartDeathScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public override void UpdateState(GameStateManager gameStateManager)
    {
        //Reloads the scene
    }

    //IEnumerator StartDeathScene()
    //{
    //    yield return new WaitForSeconds(5f);
    //    //print("Coroutine ended: " + Time.time + " seconds");
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
}
