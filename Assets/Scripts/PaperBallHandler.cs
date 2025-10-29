using System.Collections;
using UnityEngine;
using FMODUnity;

public class PaperBallHandler : MonoBehaviour
{
    private float destructionTimer = 3.5f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponentInParent<EnemyStateManager>() != null)
        {
            if(collision.gameObject.GetComponentInParent<EnemyStats>().canResetFromThrowable)
            {
                collision.gameObject.GetComponentInParent<EnemyStateManager>().SwitchState(collision.gameObject.GetComponentInParent<EnemyStateManager>().enemyIdleState);
                RuntimeManager.PlayOneShot(FMODEvents.instance.paperImpact, this.gameObject.transform.position);
                StartCoroutine(SelfDestruct());
            }
        }
    }

    /// <summary>
    /// Destroys this object after a certain amount of time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(destructionTimer);
        Destroy(gameObject);
    }
}
