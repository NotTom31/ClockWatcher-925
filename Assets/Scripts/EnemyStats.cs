using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float stalkChance;
    private float stalkChanceLevelIncrementAmount = 0.05f;

    public float currentStalkTime;
    public float TimeBeforeAttack;
    private float timeBeforeAttackLevelIncrementAmount = 2f;

    public float TimeBeforeRetryToStalk;
    public float currentRetryWaitTime;
    private float timeBeforeRetryToStalkLevelIncrementAmount = 2f;

    public bool failedToStalk;
    public bool canResetFromThrowable;


    /// <summary>
    /// Updates the current stat based on the current level.
    /// </summary>
    /// <param name="level">Build index of the scene.</param>
    public void UpdateStats(int level)
    {
        stalkChance = stalkChance + ((level-1) * stalkChanceLevelIncrementAmount);
        TimeBeforeAttack = TimeBeforeAttack - ((level-1) * timeBeforeAttackLevelIncrementAmount);
        TimeBeforeRetryToStalk = TimeBeforeRetryToStalk - ((level-1) * timeBeforeRetryToStalkLevelIncrementAmount);
    }
}
