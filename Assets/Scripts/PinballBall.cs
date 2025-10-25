using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PinballBall : MonoBehaviour
{
    [Header("Physics Settings")]
    public float maxSpeed = 20f;
    public float minSpeed = 2f;
    public float nudgeForce = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        Vector2 currentVel = rb.linearVelocity;

        float speed = currentVel.magnitude;
        if (speed > maxSpeed)
        {
            rb.linearVelocity = currentVel.normalized * maxSpeed;
        }
        else if (speed < minSpeed && speed > 0.1f)
        {
            rb.linearVelocity = currentVel.normalized * minSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision (sound, effects)
    }

    public void Nudge(Vector2 direction)
    {
        rb.AddForce(direction.normalized * nudgeForce, ForceMode2D.Impulse);
    }
}
