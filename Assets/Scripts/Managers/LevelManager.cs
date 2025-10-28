using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IDataPersistance
{
    private float checkTimer;
    private float checkInterval = 3f;

    public TimeSpan currentTime = new TimeSpan();
    public TimeSpan clockInTime = new TimeSpan(8, 00, 00);
    public TimeSpan clockOutTime = new TimeSpan(4, 30, 00);

    public int currentLevel = 1;
    public int gameDifficulty;

    public IEnumerable<EnemyStats> enemyStats;

    public static LevelManager instance;

    public bool switchingStates;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Level Manager. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        switchingStates = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = clockInTime;

        enemyStats = FindObjectsByType<EnemyStats>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        ModifiyEnviromentStats();

        ComputerManager.instance.UpdateTime(currentTime.ToString());
    }

    public string GetTime()
    {
        return currentTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    /// <summary>
    /// Updates the "Time" in the game for the analog clock
    /// </summary>
    private void UpdateTime()
    {
        checkTimer += Time.deltaTime;

        //See if timer has passed the checkInterval
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;

            currentTime = currentTime.Add(TimeSpan.FromMinutes(10));

            ComputerManager.instance.UpdateTime(currentTime.ToString());
        }

        if(currentTime == clockOutTime)
        {
            if (checkForUnfinishedTasks() && switchingStates == false)
            {
                switchingStates = true;

                GameStateManager.instance.SwitchState(GameStateManager.instance.gameWinState);
            }
        }
    }

    /// <summary>
    /// Checks and hanldes at the end of day for the tasks.
    /// </summary>
    public bool checkForUnfinishedTasks()
    {
        //TODO Write the logic for checking tasks at end of day.

        return true;
    }

    /// <summary>
    /// Modifies each enemy stats based on the current level.
    /// </summary>
    public void ModifiyEnviromentStats()
    {
        foreach(EnemyStats enemy in enemyStats)
        {
            enemy.UpdateStats(gameDifficulty);
        }
    }

    public void DisableEnemies()
    {
        foreach(var enemy in enemyStats)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    public void LoadData(GameData data)
    {
        this.gameDifficulty = data.gameDifficulty;
    }
    
    public void SaveData(GameData data)
    {
        data.gameDifficulty = this.gameDifficulty;
    }
}
