using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// This object handles all logic for one specific instance of one minigame.
// The logic to handle any given minigame should take place in an extension of this abstract class.
public abstract class MinigameLogic : MonoBehaviour
{
    [SerializeField] 
    protected List<GameObject> prefabs;             // list of entity prefabs for instantiation. Populate this list inside a minigame-specific Prefab.
    protected List<GameObject> entities = new();    // in-game entities handled by this minigame
    public int minigameID;                          // referenced by MinigamesManager to determine which minigame this is. Set externally.

    [SerializeField]
    int instantiateOnAwake = -1;    // If non-negative, immediately instantiates at that difficulty. For testing purposes

    private void Awake()
    {
        if (instantiateOnAwake >= 0)
        {
            InstantiateGame(instantiateOnAwake);
        }
    }

    public abstract float EvaluateScore();  // expected to return a number between 0 and 100(?)

    public abstract void InstantiateGame(int difficulty = 0); // seed the game differently based on difficulty tiers. Expected input 0-5 (?)

    public virtual void DestroyGame()
    {
        foreach (GameObject g in entities)
            Destroy(g);
        Destroy(gameObject);
    }
    
}
