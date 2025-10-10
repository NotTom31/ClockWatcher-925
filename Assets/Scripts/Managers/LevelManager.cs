using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private float checkTimer;
    private float checkInterval = 10f;

    public TimeSpan currentTime = new TimeSpan();
    private TimeSpan clockInTime = new TimeSpan(8, 00, 00);
    private TimeSpan clockOutTime = new TimeSpan(4, 00, 00);

    public int currentLevel;

    public IEnumerable<EnemyStats> enemyStats;

    public static LevelManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = clockInTime;

        currentLevel = SceneManager.GetActiveScene().buildIndex;

        enemyStats = FindObjectsByType<EnemyStats>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        ModifiyEnviromentStats();
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
            Debug.Log(currentTime);
        }

        if(currentTime == clockOutTime)
        {
            checkForUnfinishedTasks();
        }
    }

    /// <summary>
    /// Checks and hanldes at the end of day for the tasks.
    /// </summary>
    public void checkForUnfinishedTasks()
    {
        //TODO Write the logic for checking tasks at end of day.
    }

    /// <summary>
    /// Modifies each enemy stats based on the current level.
    /// </summary>
    public void ModifiyEnviromentStats()
    {
        foreach(EnemyStats enemy in enemyStats)
        {
            enemy.UpdateStats(currentLevel);
        }
    }
}
