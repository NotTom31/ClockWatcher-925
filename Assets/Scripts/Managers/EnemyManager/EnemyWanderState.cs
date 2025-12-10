using UnityEngine;
using UnityEngine.AI;

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
        enemyStateManager.model.GetComponentInChildren<Collider>().enabled = false;
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
                enemyStateManager.enemyStats.currentWaitTimerBeforeWander = enemyStateManager.enemyStats.waitTimerBeforeWander;
            }
        }
        else
        {
            enemyStateManager.enemyStats.currentWaitTimerBeforeWander -= Time.deltaTime;

            if (enemyStateManager.agent.remainingDistance == 0)
            {
                UpdatePosition(enemyStateManager);
            }
        }

        if (enemyStateManager.agent.remainingDistance == 0)
        {
            enemyStateManager.animator.Play("Idle");
        }
        else
        {
            enemyStateManager.animator.Play("Walk");
        }
    }

    /// <summary>
    /// Handles any logic that needs to be done before leaving the state
    /// </summary>
    /// <param name="enemyStateManager">The Game State Manager</param>
    public override void OnExit(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.model.GetComponentInChildren<Collider>().enabled = true;
    }

    private bool CalculateDisappearChance(float disappearChance)
    {
        float random = Random.value;

        // Check to see if we rolled a stalk then transition to another state
        if (random < disappearChance)
        {
            Debug.Log("Disappear..." + random);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdatePosition(EnemyStateManager enemyStateManager)
    {
        //TODO Update Position to NavMesh
        if(!enemyStateManager.agent.hasPath)
        {
            Vector3 dest = RandomNavmeshLocation(enemyStateManager,20f);
            enemyStateManager.agent.SetDestination(dest);
        }
    }

    public static Vector3 RandomNavmeshLocation(EnemyStateManager enemyStateManager, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += enemyStateManager.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return enemyStateManager.transform.position; // fallback
    }
}
