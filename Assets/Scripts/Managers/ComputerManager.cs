using UnityEngine;

public class ComputerManager : Interactable
{
    public static ComputerManager instance { get; private set; }
    public Transform cameraPosition;

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D grabCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public override void Interact()
    {
        //toggles the player being on the computer
        PlayerManager.instance.onComputer = !PlayerManager.instance.onComputer;

        if (PlayerManager.instance.onComputer)
        {
            //unlocks the mouse and make it visable and set the camera target position to the computer
            CameraManager.instance.setMouseLockState(false);
            CameraManager.instance.targetTransform = cameraPosition;
        }
        else
        {
            //Locks the mouse and make it invisable and set the camera target position to the center of the "room"
            CameraManager.instance.setMouseLockState(true);
            CameraManager.instance.targetTransform = CameraManager.instance.orientation;
        }
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, cursorHotspot, CursorMode.Auto);
    }

    public void SetGrabCursor()
    {
        Cursor.SetCursor(grabCursor, cursorHotspot, CursorMode.Auto);
    }
}
