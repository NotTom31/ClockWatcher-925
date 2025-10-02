using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [SerializeField] private bool disableDataPersistance = false;

    [SerializeField] private bool overrideSelectedProfileId = false;

    [SerializeField] private string testSelectedProfileID = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption = false;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private GameData gameData;

    private List<IDataPersistance> dataPersistancesObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileID = string.Empty;

    private Coroutine autoSaveCoroutine;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        //Check to see if this exists in the scene, delete this object if one exists.
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistance Manager. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        //Set it to not destory on load to persist to other scenes.
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistance)
        {
            Debug.LogWarning("Data Persistence is currently disabled.");
        }

        //Set the path for saving the .game file.
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    /// <summary>
    /// Creates a new game with a blank Gamedata file.
    /// </summary>
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    /// <summary>
    /// Loads in data from the dataHanlder and if it is null, creates a new game.
    /// </summary>
    public void LoadGame()
    {
        //Do not load if debugging tool is enabled.
        if (disableDataPersistance)
        {
            return;
        }
        //load game file from the selected profile in the directory.
        this.gameData = dataHandler.Load(selectedProfileID);
        if (this.gameData == null && initializeDataIfNull)
        {
            Debug.Log("New Game has been started.");
            NewGame();
        }
        if (this.gameData == null)
        {
            Debug.Log("No data was Found. A New game needs to be started before data can be loaded.");
            return;
        }
        //Load all the data from the gameData file into each object in the scene.
        foreach (IDataPersistance dataPersistance in dataPersistancesObjects)
        {
            dataPersistance.LoadData(gameData);
        }
    }

    /// <summary>
    /// Save the current game to the save file.
    /// </summary>
    public void SaveGame()
    {
        //Do not save if debugging tool is enabled.
        if (disableDataPersistance)
        {
            return;
        }
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New game needs to be started before data can be saved.");
            return;
        }
        //save all the data from the gameData file into each object in the scene.
        foreach (IDataPersistance dataPersistance in dataPersistancesObjects)
        {
            dataPersistance.SaveData(gameData);
        }
        //Save the current time into the file for tracking.
        gameData.lastUpdated = DateTime.Now.ToBinary();

        //Call the function to write the save file.
        dataHandler.Save(gameData, selectedProfileID);
    }

    /// <summary>
    /// Delete a selected game file based off of profile ID.
    /// </summary>
    /// <param name="profileID"></param>
    public void DeleteGame(string profileID)
    {
        dataHandler.Delete(profileID);

        InitializeSelectedProfileId();

        LoadGame();
    }

    /// <summary>
    /// Sets the profile ID to the most recently updated game file.
    /// </summary>
    private void InitializeSelectedProfileId()
    {
        this.selectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();

        //If debugging tool is enabled, override the ID to be the Debugging tool ID.
        if (overrideSelectedProfileId)
        {
            this.selectedProfileID = testSelectedProfileID;
            Debug.LogWarning("Overrode selected profile ID with: " + testSelectedProfileID);
        }
    }

    /// <summary>
    /// When the scene loads, this function will run.
    /// </summary>
    /// <param name="scene">The actively ran scene.</param>
    /// <param name="mode">Adding or single scene mode.</param>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Find all the objects in the scene that save or load data.
        this.dataPersistancesObjects = FindAllDataPersistenceObjects();

        //Load the data into the objects.
        LoadGame();

        //Check to see if the Autosave function is running then starts it.
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(Autosave());
    }

    /// <summary>
    /// Finds all the object in the scene that are going to be loaded or saved.
    /// </summary>
    /// <returns></returns>
    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> sceneDataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistance>();

        return new List<IDataPersistance>(sceneDataPersistenceObjects);
    }

    /// <summary>
    /// Returns if GameData has a value
    /// </summary>
    /// <returns></returns>
    public bool HasGameData()
    {
        return gameData != null;
    }

    /// <summary>
    /// Gets all the game files and their Ids.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.loadAllProfiles();
    }

    /// <summary>
    /// Autosaves the current save file based on a time limit.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Autosave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved Game.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
