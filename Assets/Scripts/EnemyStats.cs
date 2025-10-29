using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("General stats")]
    public float moveSpeed = 0.5f;
    public int MonsterID; //0 = wag, 1 = crawler

    [Header("Stalk stats")]

    public float stalkChance;
    private float stalkChanceLevelIncrementAmount = 0.05f;

    public float currentStalkTime;
    public float TimeBeforeAttack;
    private float timeBeforeAttackLevelIncrementAmount = 2f;

    public float TimeBeforeRetryToStalk;
    public float retryWaitTime;
    public float currentRetryWaitTime;
    private float timeBeforeRetryToStalkLevelIncrementAmount = 2f;

    public bool failedToStalk;
    public bool canResetFromThrowable;


    [Header("Wander stats")]
    public bool canWander;
    public float wanderChance;

    public float currentWaitTimerBeforeWander;
    public float waitTimerBeforeWander;
    public float disappearingChance;

    private float timeWhileDisappeareddIncreaseAmount = 0.05f;
    private float wanderChanceLevelIncrementAmount = 0.05f;
    private float waitTimerBeforeWanderIncrementAmount = 0.05f;
    private float disappearingChanceIncreaseAmount = 0.05f;


    [Header("Disappear stats")]
    public float timeWhileDisappeared;
    public float currentDisappearedTime;


    public Transform currentWanderPoint;
    public int indexForPoint = 0;
    public List<Transform> wandersPositions;


    /// <summary>
    /// Updates the current stat based on the current level.
    /// </summary>
    /// <param name="level">Build index of the scene.</param>
    public void UpdateStats(int level)
    {
        stalkChance = stalkChance + ((level-1) * stalkChanceLevelIncrementAmount);
        TimeBeforeAttack = TimeBeforeAttack - ((level-1) * timeBeforeAttackLevelIncrementAmount);
        TimeBeforeRetryToStalk = TimeBeforeRetryToStalk - ((level-1) * timeBeforeRetryToStalkLevelIncrementAmount);

        wanderChance = wanderChance - ((level - 1) * wanderChanceLevelIncrementAmount);
        disappearingChance = disappearingChance + ((level - 1) * disappearingChanceIncreaseAmount);
        timeWhileDisappeared = timeWhileDisappeared - ((level - 1) * timeWhileDisappeareddIncreaseAmount);
        waitTimerBeforeWander = waitTimerBeforeWander - ((level - 1) * waitTimerBeforeWanderIncrementAmount);
    }
}
