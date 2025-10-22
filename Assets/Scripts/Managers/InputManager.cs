using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Inputs")]
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool interact_Input;
    public bool pause_Input;
    public bool shoot_Input;

    [Header("Flags")]
    public bool pauseFlag;

    InputSystem_Actions inputActions;
    GameStateManager gameStateManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    public static InputManager instance; 
    private void Awake()
    {
        gameStateManager = FindFirstObjectByType<GameStateManager>();

        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        if(CameraManager.instance != null)
        {
            //If the player is not on the computer or being jumpscared, Handle the camera movement in 3D.
            if(!PlayerManager.instance.onComputer && !PlayerManager.instance.jumpScaring)
            {
               CameraManager.instance.HandleMovement(delta, mouseX, mouseY);
            }
        }
    }

    /// <summary>
    /// If this compenent is enabled, subscribe to input events, assigns them to scriptable values and enable the Input Actions object
    /// </summary>
    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();
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
        pause_Input = false;
        shoot_Input = false;
    }

    /// <summary>
    /// Handles the inputs for every frame tick 
    /// </summary>
    /// <param name="delta">The interval in seconds from the last frame.</param>
    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleInteractInput();
        HandlePauseInput();
        HandleShootInput();
    }

    /// <summary>
    /// Handles the movement keys and camera keys from the Input Actions object and assigns them to our scriptable value.
    /// <param name="delta"></param>
    private void MoveInput(float delta)
    {
        //Remove ability to read inputs if being jumpscared.
        if(!PlayerManager.instance.jumpScaring)
        {
            //Recieve the raw values for the inputs
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
    }

    /// <summary>
    /// Subscribes and handles the interaction button input.
    /// </summary>
    private void HandleInteractInput()
    {
        inputActions.Player.Interact.performed += i => interact_Input = true;
        //Check if the player is interacting and if the game is not paused.
        if (interact_Input && !pauseFlag)
        {
            CameraManager.instance.HandleRayCastInteractPressed();
        }
    }

    /// <summary>
    /// Checks for player input to the pause button and pauses/unpauses the game.
    /// </summary>
    private void HandlePauseInput()
    {
        inputActions.Player.Pause.performed += i => pause_Input = true;

        if (pause_Input)
        {
            //toggle pauseFlag
            pauseFlag = !pauseFlag;

            //checks the pause state or resume state to the gameStateManager.
            if (pauseFlag)
            {
                gameStateManager.SwitchState(gameStateManager.gamePauseState);
            }
            else
            {
                gameStateManager.SwitchState(gameStateManager.gameResumeState);
            }
        }
    }

    /// <summary>
    /// Handles throwing the paper ball.
    /// </summary>
    private void HandleShootInput()
    {
        inputActions.Player.Shoot.performed += i => shoot_Input = true;
        if(shoot_Input)
        {
            if (PlayerManager.instance.holdingObject)
            {
                PlayerManager.instance.shootPaper();
            }
        }
    }
}
