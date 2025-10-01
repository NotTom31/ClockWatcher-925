using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public int levelIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        //Set the LoadLevelStat Index to this level index then load the next level.
        GameStateManager.instance.gameLoadLevelState.levelIndex = levelIndex;
        GameStateManager.instance.SwitchState(GameStateManager.instance.gameLoadLevelState);
    }
}
