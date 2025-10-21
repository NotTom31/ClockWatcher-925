public class EnemyAttackingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
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
