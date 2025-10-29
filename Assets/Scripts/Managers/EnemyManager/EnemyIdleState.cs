
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    //Check every checkInterval in seconds to roll a chance for the monster to start to stalk.
    private float moveSpeed = 1f;

    public override void EnterState(EnemyStateManager enemyStateManager)
    {

        //Check to see if we have failed to stalk in the past, if so, put a timer before rolling chance to stalk again.
        if(enemyStateManager.enemyStats.failedToStalk)
        {
            enemyStateManager.enemyStats.currentRetryWaitTime = enemyStateManager.enemyStats.TimeBeforeRetryToStalk;
        }
        else
        {
            enemyStateManager.enemyStats.currentRetryWaitTime = enemyStateManager.enemyStats.retryWaitTime;
        }

        enemyStateManager.animator.Play("Idle");
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    { 
        //Wait if failed to stalk, otherwise roll a chance to stalk and switch states.
        if(enemyStateManager.enemyStats.currentRetryWaitTime <= 0)
        {
            if (CalculateStalkChance(enemyStateManager.enemyStats.stalkChance))
            {
                enemyStateManager.SwitchState(enemyStateManager.enemyStalkingState);
            }
            else
            {
                if (enemyStateManager.enemyStats.canWander)
                {
                    if (CalculateWanderChance(enemyStateManager.enemyStats.wanderChance))
                    {
                        enemyStateManager.SwitchState(enemyStateManager.enemyWanderState);
                    }
                }
            }
            enemyStateManager.enemyStats.currentRetryWaitTime = enemyStateManager.enemyStats.retryWaitTime;
        }
        else
        {
            enemyStateManager.enemyStats.currentRetryWaitTime -= Time.deltaTime;
        }
        UpdatePosition(enemyStateManager);
    }

    public override void OnExit(EnemyStateManager enemyStateManager)
    {
        //Reset variables.
        enemyStateManager.enemyStats.failedToStalk = false;
        enemyStateManager.enemyStats.currentRetryWaitTime = 0f;
    }

    private bool CalculateStalkChance(float stalkChance)
    {
        //See if timer has passed the checkInterval
        float random = Random.value;

        // Check to see if we rolled a stalk then transition to another state
        if (random < stalkChance)
        {
            Debug.Log("Stalking..." + random);
            return true;
        }
        else
        {
            return false;
        }
    
    }

    private bool CalculateWanderChance(float wanderChance)
    {
        float random = Random.value;

        // Check to see if we rolled a stalk then transition to another state
        if (random < wanderChance)
        {
            Debug.Log("Wandering..." + random);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdatePosition(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.model.transform.position = Vector3.Lerp(enemyStateManager.model.transform.position, enemyStateManager.idlePosition.transform.position, moveSpeed * Time.deltaTime);
    }
}
