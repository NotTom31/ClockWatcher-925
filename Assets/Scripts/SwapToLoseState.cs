using System.Collections;
using UnityEngine;
using FMODUnity;

public class SwapToLoseState : MonoBehaviour
{
    public void OnEnable()
    {
        GameStateManager.instance.SwitchState(GameStateManager.instance.gameLoseState);
    }
}
