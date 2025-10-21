public class EnemyAttackingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        //Set the player to being jump scared and make the camera look the jump scare location.
        PlayerManager.instance.jumpScaring = true;
        CameraManager.instance.targetTransform = enemyStateManager.jumpScareOrientation;
        UIManager.instance.ToggleDeathUI();

    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {

    }

    public override void OnExit(EnemyStateManager enemyStateManager)
    {

    }

}
