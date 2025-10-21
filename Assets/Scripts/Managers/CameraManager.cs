using UnityEngine;
public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    public float sensX = 20f;
    public float sensY = 20f;

    float lookAngle;
    float pivotAngle;
    public float miniumPivot = -35f;
    public float maximumPivot = 35f;

    public float moveSpeedToComputer = 0.05f;
    public float rotateSpeedToComputer = 30f;

    public float cameraYMinClamp = -60;
    public float cameraYMaxClamp = 60;

    public Transform orientation;
    public Transform targetTransform;

    [Header("Raycast Settings")]

    public LayerMask layerMask;
    public float rayCastDistance = 5f;

    [Header("Object Holding")]
    public Transform objectHolder;

    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

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
        InputManager.instance.TickInput(Time.deltaTime);
        RepositionCamera(targetTransform);
        HandleRayCastInteractUI();
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

        //Clamp the X axis to prevent looking past certain angles.
        rotation.x = Mathf.Clamp(pivotAngle, cameraYMinClamp, cameraYMaxClamp);
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
    public void HandleRayCastInteractPressed()
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
            }
        }
    }

    public void HandleRayCastInteractUI()
    {
        //Put the raycast in the ceneter of the screen.
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;


        //Check if the object hit is on the layer mask
        if (Physics.Raycast(ray, out hit, rayCastDistance, layerMask))
        {
            //Check to see if it has the base class interactable.
            if (hit.transform.GetComponent<Interactable>() != null)
            {
                UIManager.instance.ToggleInteractUI(true);
                UIManager.instance.SetUIText(hit.transform.GetComponent<Interactable>().uiText);
            }
            else
            {
                UIManager.instance.ToggleInteractUI(false);
                UIManager.instance.SetUIText();
            }
        }
        else
        {
            UIManager.instance.ToggleInteractUI(false);
            UIManager.instance.SetUIText();
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
            //Get the difference from the quaternions to adjust looking angle
            Quaternion lookOnLook = Quaternion.LookRotation(targetTransform.transform.position - transform.position);

            //If the player is being jumpscared, look at jumpscare position.
            if (PlayerManager.instance.jumpScaring)
            {
                transform.SetPositionAndRotation(Vector3.Slerp(transform.position, orientation.transform.position, moveSpeedToComputer * Time.deltaTime), Quaternion.Slerp(transform.rotation, lookOnLook, rotateSpeedToComputer  * Time.deltaTime));
            }
            //not being jumpscared, look at the computer position.
            else
            {
                //TODO May need to recalculate the rotation as well.
                transform.SetPositionAndRotation(Vector3.Slerp(transform.position, targetTransform.transform.position, moveSpeedToComputer * Time.deltaTime), Quaternion.Slerp(transform.rotation, targetTransform.transform.rotation, rotateSpeedToComputer * Time.deltaTime));
            }
        }
    }
}
