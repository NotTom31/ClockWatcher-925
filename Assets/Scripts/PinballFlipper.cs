using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HingeJoint2D))]
public class PinballFlipperTapStable : MonoBehaviour
{
    [Header("Flipper Settings")]
    public float restAngle = 0f;      // resting rotation
    public float flickAngle = 60f;    // rotation when flicked
    public float motorSpeed = 1000f;  // speed of rotation
    public float motorTorque = 20000f;
    public float tapDuration = 0.1f;  // how long the flipper stays up

    private HingeJoint2D hinge;
    private JointMotor2D motor;
    private bool isPressed;
    private float targetAngle;

    void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        hinge.useMotor = true;
        hinge.useLimits = true;

        // Set limits
        JointAngleLimits2D limits = hinge.limits;
        limits.min = restAngle;
        limits.max = flickAngle;
        hinge.limits = limits;

        // Setup motor
        motor = hinge.motor;
        motor.maxMotorTorque = motorTorque;

        // Start at rest
        targetAngle = restAngle;
        isPressed = false;
    }

    void FixedUpdate()
    {
        float currentAngle = hinge.jointAngle;
        float angleDiff = targetAngle - currentAngle;

        // Only apply motor if flipper hasn't reached target
        if (Mathf.Abs(angleDiff) > 0.5f) // tolerance to stop twitching
        {
            motor.motorSpeed = Mathf.Sign(angleDiff) * motorSpeed;
            hinge.motor = motor;
        }
        else
        {
            // Stop the motor when target is reached
            motor.motorSpeed = 0f;
            hinge.motor = motor;
        }
    }

    // Called by UI Button OnClick()
    public void TapFlipper()
    {
        if (!isPressed)
        {
            isPressed = true;
            targetAngle = flickAngle;
            CancelInvoke(nameof(ReturnFlipper));
            Invoke(nameof(ReturnFlipper), tapDuration);
        }
    }

    private void ReturnFlipper()
    {
        isPressed = false;
        targetAngle = restAngle;
    }
}
