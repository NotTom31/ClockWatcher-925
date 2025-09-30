using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

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
        if(instance != null)
        {
            Debug.Log("Found more than one Data Persistance Manager. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if(disableDataPersistance)
        {
            Debug.LogWarning("Data Persistence is currently disabled.");
        }

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
        if (disableDataPersistance)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileID);
        if(this.gameData == null && initializeDataIfNull)
        {
            Debug.Log("New Game has been started.");
            NewGame();
        }

        if(this.gameData == null)
        {
            Debug.Log("No data was Found. A New game needs to be started before data can be loaded.");
            return;
        }
        foreach(IDataPersistance dataPersistance in dataPersistancesObjects) 
        {
            dataPersistance.LoadData(gameData);
        }
    }
    /// <summary>
    /// Save the current game to the save file.
    /// </summary>
    public void SaveGame()
    {
        if (disableDataPersistance)
        {
            return;
        }
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New game needs to be started before data can be saved.");
            return;
        }
        foreach(IDataPersistance dataPersistance in dataPersistancesObjects)
        {
            dataPersistance.SaveData(gameData);
        }
        gameData.lastUpdated = System.DateTime.Now.ToBinary();
        dataHandler.Save(gameData, selectedProfileID);
    }

    public void DeleteGame(string profileID)
    {
        dataHandler.Delete(profileID);

        InitializeSelectedProfileId();

        LoadGame();

    }
    
    /// <summary>
    /// 
    /// </summary>
    private void InitializeSelectedProfileId()
    {
        this.selectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileID = testSelectedProfileID;
            Debug.LogWarning("Overrode selected profile ID with: " + testSelectedProfileID);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called.");
        this.dataPersistancesObjects = FindAllDataPersistenceObjects();
        LoadGame();

        if(autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(Autosave());
    }

    /// <summary>
    /// Finds all the object in the scene that are going to be saved.
    /// </summary>
    /// <returns></returns>
    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> sceneDataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistance>();

        return new List<IDataPersistance>(sceneDataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.loadAllProfiles();
    }

    private IEnumerator Autosave()
    {
        while(true)
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
        Debug.Log("DataPersistenceManager Enabled.");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        Debug.Log("DataPersistenceManager disabled.");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
