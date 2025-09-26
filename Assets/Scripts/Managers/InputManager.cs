using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool interact_Input;

    InputSystemActions inputActions;

    Vector2 movementInput;
    Vector2 cameraInput;

    /// <summary>
    /// If this compenent is enabled, subscribe to input events, assigns them to scriptable values and enable the Input Actions object
    /// </summary>
    public void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new InputSystemActions();
            inputActions.Player.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }

    /// <summary>
    /// If this compenent is disabled, unsubscribe to input events and disable Input Actions object.
    /// </summary>
    private void OnDisable()
    {
        inputActions.Disable();
    }

    /// <summary>
    /// Turns off the flags at the end of the Game Logic code to prevent "double" inputs or "no" inputs
    /// </summary>
    private void LateUpdate()
    {
        interact_Input = false;
    }

    /// <summary>
    /// Handles the inputs from every frame tick 
    /// </summary>
    /// <param name="delta">The interval in seconds from the last frame.</param>
    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleInteractInput();
    }

    /// <summary>
    /// Handles the movement keys and camera keys from the Input Actions object and assigns them to our scriptable value.
    /// <param name="delta"></param>
    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    /// <summary>
    /// Subscribes and handles the interaction button inut.
    /// </summary>
    private void HandleInteractInput()
    {
        inputActions.Player.Interact.performed += i => interact_Input = true;
        if(interact_Input)
        {
            Debug.Log("Interact button has been pressed.");

        }
    }
}
