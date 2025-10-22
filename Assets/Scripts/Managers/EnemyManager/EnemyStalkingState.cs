using UnityEngine;

public class EnemyStalkingState : EnemyBaseState
{
    private float moveSpeed = 1;
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        //Sets stalk countdown
        enemyStateManager.enemyStats.currentStalkTime = enemyStateManager.enemyStats.TimeBeforeAttack;
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
        enemyStateManager.model.transform.position = Vector3.Lerp(enemyStateManager.model.transform.position, enemyStateManager.stalkingPosition.transform.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(enemyStateManager.model.transform.position, enemyStateManager.stalkingPosition.transform.position) > 0.1f)
        {
            enemyStateManager.animator.Play("Walk");
        }
        else
        {
            enemyStateManager.animator.Play("Idle");
        }
    }
}
