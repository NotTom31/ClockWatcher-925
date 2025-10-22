using System.Diagnostics;

public class EnemyAttackingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        Debug.WriteLine("Entered Attacking State");
        //Set the player to being jump scared and make the camera look the jump scare location.
        PlayerManager.instance.jumpScaring = true;
        UIManager.instance.ToggleDeathUI();
        CameraManager.instance.targetTransform = enemyStateManager.jumpScareOrientation;

    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {

    }

    public override void OnExit(EnemyStateManager enemyStateManager)
    {

    }

}
