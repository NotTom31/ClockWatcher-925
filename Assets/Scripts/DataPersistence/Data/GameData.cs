using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public int interactedCount;

    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> interactionsDone;

    /// <summary>
    /// The game starts with thess values when there is no data. 
    /// Validate this is where the values are need to be on a new game aka when you first load into the scene.
    /// </summary>
    public GameData()
    {
        this.interactedCount = 0;
        playerPosition = Vector3.zero;
        interactionsDone = new SerializableDictionary<string, bool>();
    } 

    /// <summary>
    /// Returns a number of how far the game file is.
    /// </summary>
    /// <returns></returns>
    public int GetPercentageCompelted()
    {
        //TODO Create a way to calculate how far the player is.
        return 0;
    }
}
