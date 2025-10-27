/*using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PinballBall : MonoBehaviour
{
    [Header("Physics Settings")]
    public float maxSpeed = 20f;
    public float minSpeed = 2f;
    public float nudgeForce = 5f;

    [Header("Scale Compensation")]
    [Tooltip("Set this to your ball's world scale (e.g., 0.1 if it's 10x smaller than normal)")]
    public float worldScale = 0.1f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Compensate for small world size — reduce gravity’s effect
        rb.gravityScale *= worldScale;
    }

    void FixedUpdate()
    {
        Vector2 currentVel = rb.linearVelocity;

        // Apply scaled speed limits
        float scaledMax = maxSpeed * worldScale;
        float scaledMin = minSpeed * worldScale;

        float speed = currentVel.magnitude;

        if (speed > scaledMax)
        {
            rb.linearVelocity = currentVel.normalized * scaledMax;
        }
        else if (speed < scaledMin && speed > 0.1f * worldScale)
        {
            rb.linearVelocity = currentVel.normalized * scaledMin;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void Nudge(Vector2 direction)
    {
        // Scale impulses so they feel natural at small scale
        rb.AddForce(direction.normalized * nudgeForce * worldScale, ForceMode2D.Impulse);
    }
}
*/