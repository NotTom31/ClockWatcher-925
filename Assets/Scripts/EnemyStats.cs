using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float stalkChance;

    public float currentStalkTime;
    public float TimeBeforeAttack;

    public float TimeBeforeRetryToStalk;
    public float currentRetryWaitTime;

    public bool failedToStalk;
}
