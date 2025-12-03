using UnityEngine;
using UnityEngine.AI;

public class EnemyStalkingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        //Sets stalk countdown
        enemyStateManager.enemyStats.currentStalkTime = enemyStateManager.enemyStats.TimeBeforeAttack;
        enemyStateManager.agent.SetDestination(enemyStateManager.stalkingPosition.transform.position);

    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        UpdateStalkTime(enemyStateManager);
        UpdatePosition(enemyStateManager);
    }

    public override void OnExit(EnemyStateManager enemyStateManager)
    {
        //Reset stalk variables.
        enemyStateManager.enemyStats.currentStalkTime = 0;
        enemyStateManager.enemyStats.currentRetryWaitTime = enemyStateManager.enemyStats.TimeBeforeRetryToStalk;
    }

    /// <summary>
    /// Manages the enemy stalking time and handles switching states.
    /// </summary>
    /// <param name="enemyStateManager"></param>
    public void UpdateStalkTime(EnemyStateManager enemyStateManager)
    {
        //Reduces timing before attacking
        enemyStateManager.enemyStats.currentStalkTime -= Time.deltaTime;

        //See if the item the player interacts with is enabled, if so, stalking fails.
        if(enemyStateManager.interactable.interactableEnabled)
        {
            enemyStateManager.enemyStats.failedToStalk = true;
            enemyStateManager.SwitchState(enemyStateManager.enemyIdleState);
        }
        else if (enemyStateManager.enemyStats.currentStalkTime <= 0)
        {
            enemyStateManager.SwitchState(enemyStateManager.enemyAttackingState);
        }
    }

    /// <summary>
    /// Hanldes updating the enemy models position to a new position.
    /// </summary>
    /// <param name="enemyStateManager"></param>
    public void UpdatePosition(EnemyStateManager enemyStateManager)
    {
        if (enemyStateManager.agent.remainingDistance == 0)
        {
            enemyStateManager.animator.Play("Idle");
        }
        else
        {
            enemyStateManager.animator.Play("Walk");
        }
    }
}
