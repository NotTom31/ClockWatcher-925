using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState;

    public EnemyAttackingState enemyAttackingState = new EnemyAttackingState();
    public EnemyStalkingState enemyStalkingState = new EnemyStalkingState();
    public EnemyWanderState enemyWanderState = new EnemyWanderState();
    public EnemyIdleState enemyIdleState = new EnemyIdleState();
    public EnemyDisappearingState enemyDisappearingState = new EnemyDisappearingState();


    public EnemyStats enemyStats;

    public Transform jumpScareOrientation;
    public Transform stalkingPosition;
    public Transform idlePosition;
   
    public Interactable interactable;
    
    public GameObject model;
    public Animator animator;
     
    private void Awake()
    {
        //Destroy second instance of GameStateManager if it exists.
        enemyStats = GetComponent<EnemyStats>();
        animator = GetComponentInChildren<Animator>();  
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
