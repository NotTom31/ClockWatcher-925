using UnityEngine;

public class EnemyDisappearingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.enemyStats.currentDisappearedTime = enemyStateManager.enemyStats.timeWhileDisappeared;
        enemyStateManager.model.transform.position = enemyStateManager.idlePosition.position;
        enemyStateManager.model.SetActive(false);
        Debug.Log("Entered Disppeared.");

    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        if (enemyStateManager.enemyStats.currentDisappearedTime <= 0)
        {
            enemyStateManager.enemyStats.currentDisappearedTime = 0;
            enemyStateManager.SwitchState(enemyStateManager.enemyIdleState);
        }
        else
        {
            enemyStateManager.enemyStats.currentDisappearedTime -= Time.deltaTime;
        }
    }
    public override void OnExit(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.model.SetActive(true);
    }
}
