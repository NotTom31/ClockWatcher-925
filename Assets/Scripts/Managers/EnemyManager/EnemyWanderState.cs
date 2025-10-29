using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{
    /// <summary>
    /// Handles any logic that needs to be done when entering the state.
    /// </summary>
    /// <param name="enemyStateManager">The Game State Manager</param>
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.enemyStats.currentWaitTimerBeforeWander = enemyStateManager.enemyStats.waitTimerBeforeWander;
        enemyStateManager.enemyStats.currentWanderPoint = enemyStateManager.idlePosition;
        enemyStateManager.enemyStats.indexForPoint = 0;
    }

    /// <summary>
    /// Handles any logic that needs to be done every update frame.
    /// </summary>
    /// <param name="enemyStateManager">The Game State Manager</param>
    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        //Wait if failed to stalk, otherwise roll a chance to stalk and switch states.
        if (enemyStateManager.enemyStats.currentWaitTimerBeforeWander <= 0)
        {
            if (CalculateDisappearChance(enemyStateManager.enemyStats.disappearingChance))
            {
                enemyStateManager.SwitchState(enemyStateManager.enemyDisappearingState);

            }
            else
            {
                enemyStateManager.enemyStats.currentWanderPoint = RecalculatePosition(enemyStateManager);
                enemyStateManager.enemyStats.currentWaitTimerBeforeWander = enemyStateManager.enemyStats.waitTimerBeforeWander;
            }
        }
        else
        {
            enemyStateManager.enemyStats.currentWaitTimerBeforeWander -= Time.deltaTime;
        }

        UpdatePosition(enemyStateManager, enemyStateManager.enemyStats.currentWanderPoint);
    }

    /// <summary>
    /// Handles any logic that needs to be done before leaving the state
    /// </summary>
    /// <param name="enemyStateManager">The Game State Manager</param>
    public override void OnExit(EnemyStateManager enemyStateManager)
    {
        Debug.Log("Exiting Wandering...");
    }

    private bool CalculateDisappearChance(float disappearChance)
    {
        float random = Random.value;

        // Check to see if we rolled a stalk then transition to another state
        if (random < disappearChance)
        {
            Debug.Log("Stalking..." + random);
            return true;
        }
        else
        {
            return false;
        }
    }

    public Transform RecalculatePosition(EnemyStateManager enemyStateManager)
    {
        float random = Random.value;

        if(random <= 0.5)
        {
            enemyStateManager.enemyStats.indexForPoint -= 1;
        }
        else
        {
            enemyStateManager.enemyStats.indexForPoint += 1;
        }

        if (enemyStateManager.enemyStats.indexForPoint == enemyStateManager.enemyStats.wandersPositions.Count)
        {
            enemyStateManager.enemyStats.indexForPoint = 0;
        }
        else if(enemyStateManager.enemyStats.indexForPoint < 0)
        {
            enemyStateManager.enemyStats.indexForPoint = enemyStateManager.enemyStats.wandersPositions.Count - 1;
        }

        return enemyStateManager.enemyStats.wandersPositions[enemyStateManager.enemyStats.indexForPoint].transform;
    }

    public void UpdatePosition(EnemyStateManager enemyStateManager, Transform newPosition)
    {
        enemyStateManager.model.transform.position = Vector3.Lerp(enemyStateManager.model.transform.position, newPosition.position, enemyStateManager.enemyStats.moveSpeed * Time.deltaTime);
        if (Vector3.Distance(enemyStateManager.model.transform.position, newPosition.position) > 0.1f)
        {
            enemyStateManager.animator.Play("Walk");
        }
        else
        {
            enemyStateManager.animator.Play("Idle");
        }
    }
}
