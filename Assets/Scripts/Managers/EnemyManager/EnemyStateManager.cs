using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState;

    public EnemyAttackingState enemyAttackingState = new EnemyAttackingState();
    public EnemyStalkingState enemyStalkingState = new EnemyStalkingState();
    public EnemyIdleState enemyIdleState = new EnemyIdleState();
    public EnemyStats enemyStats;

    public Transform jumpScareOrientation;
    public Interactable interactable;

    public static EnemyStateManager instance;
    private void Awake()
    {
        //Destroy second instance of GameStateManager if it exists.
        if (instance != null)
        {
            Debug.Log("Found more than one emeny Manager. Destroying the newest one.");
            Destroy(gameObject);
            return;
        }
        instance = this;

        enemyStats = GetComponent<EnemyStats>();
    }
    public void Start()
    {
        currentState = enemyIdleState;

        currentState.EnterState(this);
    }

    public void Update()
    {
        currentState.UpdateState(this);
    }

    /// <summary>
    /// Handles exiting the current state the game is in and switches to the new state.
    /// </summary>
    /// <param name="state">The new state for the system to be on.</param>
    public void SwitchState(EnemyBaseState state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        currentState.EnterState(this);
    }

}
