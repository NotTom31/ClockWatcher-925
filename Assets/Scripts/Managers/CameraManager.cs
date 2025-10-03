using UnityEngine;
public class CameraManager : MonoBehaviour
{
    private InputManager inputManager;

    [Header("Camera Settings")]
    public float sensX = 20f;
    public float sensY = 20f;

    float lookAngle;
    float pivotAngle;
    public float miniumPivot = -35f;
    public float maximumPivot = 35f;

    public float moveSpeedToComputer = 0.05f;
    public float rotateSpeedToComputer = 0.05f;

    public Transform orientation;
    public Transform targetTransform;

    [Header("Raycast Settings")]

    public LayerMask layerMask;
    public float rayCastDistance = 5f;

    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        inputManager = FindAnyObjectByType<InputManager>();

        //Sets the raycast mask layer.
        layerMask = LayerMask.GetMask("Interactables");

        //Set the cameras target position to the "center" of the room.
        targetTransform = orientation;

    }
    private void Start()
    {
        setMouseLockState(true);
    }

    public void Update()
    {
        inputManager.TickInput(Time.deltaTime);
        RepositionCamera(targetTransform);
    }

    /// <summary>
    /// Calcultate the player's input to camera position.
    /// </summary>
    /// <param name="delta">Time difference</param>
    /// <param name="mouseXInput">Player's Mouse X input</param>
    /// <param name="mouseYInput">Player's Mouse Y input</param>
    public void HandleMovement(float delta, float mouseXInput, float mouseYInput)
    {
        //Caluculate the angle for the camera to look at
        lookAngle += (mouseXInput * sensX) * delta;
        //Caluculate the pivot for the camera to look at
        pivotAngle -= (mouseYInput * sensY) * delta;

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        rotation.x = pivotAngle;
        //Rotate the Camera to the desired direction. 
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
    }

    /// <summary>
    /// Handles when to show/hide the cursor and to lock/unlock it.
    /// </summary>
    /// <param name="lockState">True locks the cursor and hides it. False unlocks it and shows it.</param>
    public void setMouseLockState(bool lockState)
    {
        if (lockState)
        {
            //If the player is not the computer, lock state can lock
            if (!PlayerManager.instance.onComputer)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// Handles the players ability to see items in 3D mode.
    /// </summary>
    public void HandleRayCast()
    {
        //Put the raycast in the ceneter of the screen.
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        //Check if hte object hit is on the layer mask
        if (Physics.Raycast(ray, out hit, rayCastDistance, layerMask))
        {
            //Check to see if it has the base class interactable.
            if (hit.transform.GetComponent<Interactable>() != null)
            {
                //Calls the Interace function on the class
                hit.transform.GetComponent<Interactable>().Interact();

                Debug.Log(hit.transform.gameObject.name);
            }
        }
    }

    /// <summary>
    /// Repositions the camera to the selected transform.
    /// </summary>
    /// <param name="targetTransform">Targeted Position and Rotation.</param>
    public void RepositionCamera(Transform targetTransform)
    {
        if (!InputManager.instance.pauseFlag)
        {
            transform.position = Vector3.Slerp(GetComponent<Camera>().transform.position, targetTransform.transform.position, moveSpeedToComputer);
            transform.rotation = Quaternion.Slerp(GetComponent<Camera>().transform.rotation, targetTransform.transform.rotation, rotateSpeedToComputer);
        }
    }
}
