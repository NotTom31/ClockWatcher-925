using System.Collections.Generic;
using UnityEngine;

// This class keeps track of all minigames for a given level. It can be used to add or remove minigames and tallies their scores.
// It is expected that this manager will be destroyed and re-instantiated for each level. Do not persist this manager between scenes.

public class MinigamesManager : MonoBehaviour
{
    private static MinigamesManager instance;
    public static MinigamesManager Instance { get { return instance; } }

    [SerializeField] List<GameObject> minigamePrefabs;   //list of instantiable minigames. The order you put them in here determines "gameID"
    [SerializeField] Canvas computerCanvas;
    List<MinigameData> games = new List<MinigameData>(); //references to all relevant game instances, completed or running.            

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        // initialize game ID for each tracked minigame type
        for (int ii = 0; ii < minigamePrefabs.Count; ii++)
            minigamePrefabs[ii].GetComponent<MinigameLogic>().minigameID = ii;
    }

    // Puts a minigame on computer screen of type "id" with the specified difficulty level. "localPosition" determines where on computer screen
    public void StartMinigame(int id, int difficulty, Vector2 localPosition)
    {
        // Use a prefab to instantiate this game at the desired screen position
        MinigameLogic logic = Instantiate(minigamePrefabs[id], computerCanvas.transform).GetComponent<MinigameLogic>();
        logic.transform.localPosition = localPosition;

        // Create a MinigameData to track this minigame
        MinigameData data = new MinigameData();
        data.logic = logic;
        data.difficulty = difficulty;
        data.gameID = id;
        data.score = 50f; //placeholder score
        games.Add(data);

        // Tell the minigame to set itself up
        logic.InstantiateGame(difficulty);
    }

    // Destroy a minigame. If "track" is true, a record of this minigame's data will be preserved
    // "Index" is NOT the game ID, it's the index of the game in the "games" list
    public void EndMinigame(int index, bool track = true)
    {
        MinigameLogic logic = games[index].logic;
        if (logic != null && track)
        {
            RecordScore(index);
            games[index].logic = null;
        }
        if (logic != null)
            logic.DestroyGame();
        if (!track)
            games.RemoveAt(index);
    }
    // Overload for if you have a reference but don't know the index
    public void EndMinigame(MinigameLogic logic, bool track = true)
    {
        EndMinigame(GameIndexByRef(logic), track);
    }

    public void EndAllMinigames(bool track = true)
    {
        int ii = 0;
        while (ii < games.Count)
        {
            EndMinigame(ii, track);
            if (track) //increment not needed if entry is removed
                ii++;
        }
    }

    // Checks a minigame's current score and records it
    public void RecordScore(int index)
    {
        MinigameData data = games[index];
        if (data.logic != null)
            data.score = data.logic.EvaluateScore();
    }
    public void RecordScore(MinigameLogic logic)
    {
        RecordScore(GameIndexByRef(logic));
    }

    // Finds a minigame's index if you have a reference to it
    int GameIndexByRef(MinigameLogic logic)
    {
        for (int ii = 0; ii < games.Count; ii++)
            if (logic == games[ii].logic)
                return ii;
        Debug.LogError("Minigame " + logic.ToString() + " was referenced but is not indexed by MinigamesManager");
        return -1;
    }

    // Returns the preserved data for all the minigames that have already been destroyed
    public List<MinigameData> GetCompletedMinigames()
    {
        List<MinigameData> list = new List<MinigameData>();
        foreach (MinigameData data in games)
        {
            if (data.logic == null)
                list.Add(data);
        }
        return list;
    }

    // Evaluates the score for a minigame, logs it, and returns it
    public float GetScore(int index)
    {
        RecordScore(index);
        return games[index].score;
    }
    public float GetScore(MinigameLogic logic)
    {
        return GetScore(GameIndexByRef(logic));
    }

    // Get average score of all recorded minigames
    public float GetAverageScore()
    {
        if (games.Count == 0) return 0f;
        float total = 0f;
        for (int ii = 0; ii < games.Count; ii++)
        {
            total += GetScore(ii);
        }
        return total / games.Count;
    }
}
