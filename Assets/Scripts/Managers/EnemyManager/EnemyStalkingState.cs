using UnityEngine;

public class EnemyStalkingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        //Sets stalk countdown
        enemyStateManager.enemyStats.currentStalkTime = enemyStateManager.enemyStats.TimeBeforeAttack;

    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        UpdateStalkTime(enemyStateManager);
    }

    public override void OnExit(EnemyStateManager enemyStateManager)
    {
        //Reset stalk variables.
        enemyStateManager.enemyStats.currentStalkTime = 0;
        enemyStateManager.enemyStats.currentRetryWaitTime = enemyStateManager.enemyStats.TimeBeforeRetryToStalk;
    }

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
}
