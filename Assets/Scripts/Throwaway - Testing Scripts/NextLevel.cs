using UnityEngine;

public class NextLevel : MonoBehaviour
{
    GameStateManager stateManager;

    public int levelIndex = 0;

    private void Awake()
    {
        stateManager = FindAnyObjectByType<GameStateManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        stateManager.gameLoadLevelState.levelIndex = levelIndex;  
        stateManager.SwitchState(stateManager.gameLoadLevelState);
    }
}
