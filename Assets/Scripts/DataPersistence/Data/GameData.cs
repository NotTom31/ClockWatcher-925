using UnityEngine;

//Calls Serializable to process the data into a JSON or JSON into a data object.
[System.Serializable]
public class GameData
{
    //Add data you would like to cross into new scenes or would like to be saves.
    public long lastUpdated;

    public int interactedCount;

    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> interactionsDone;

    public int sceneToLoadIndex;

    /// <summary>
    /// The game starts with thess values when there is no data. 
    /// NOTE: Validate these values are what is needed AKA playerPosition = Vector3.zero will spawn the player at 0,0,0 into the new scene.
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
    /// <returns>Percentage of game completeion.</returns>
    public float GetPercentageCompelted()
    {
        //TODO Create a way to calculate how far the player is.
        return 0;
    }
}
