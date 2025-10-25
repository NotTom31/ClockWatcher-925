using UnityEngine;
using System.Collections.Generic;

public class ComputerManager : Interactable
{
    public static ComputerManager instance { get; private set; }
    public Transform cameraPosition;

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D grabCursor;
    [SerializeField] private Texture2D dragCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    [Header("Window Settings")]
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private Transform windowParent;

    private List<GameObject> activeWindows = new List<GameObject>();

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed Extra");
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        uiText = "get on computer";
    }

    public override void Interact()
    {
        //toggles the player being on the computer
        PlayerManager.instance.onComputer = !PlayerManager.instance.onComputer;

        if (PlayerManager.instance.onComputer)
        {
            //unlocks the mouse and make it visable and set the camera target position to the computer
            CameraManager.instance.SetMouseLockState(false);
            CameraManager.instance.targetTransform = cameraPosition;
            //StartPopupMinigame(3);
        }
        else
        {
            //Locks the mouse and make it invisable and set the camera target position to the center of the "room"
            CameraManager.instance.SetMouseLockState(true);
            CameraManager.instance.targetTransform = CameraManager.instance.orientation;
        }
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, cursorHotspot, CursorMode.Auto);
    }

    public void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, cursorHotspot, CursorMode.Auto);
    }

    public void SetGrabCursor()
    {
        Cursor.SetCursor(grabCursor, cursorHotspot, CursorMode.Auto);
    }

    public void SetDragCursor()
    {
        Cursor.SetCursor(dragCursor, cursorHotspot, CursorMode.Auto);
    }


    /// <summary>
    /// Start a minigame that is intended to be cleared instead of persistant
    /// </summary>
    /// <param name="GameID">the id of the minigame to start</param>
    public void StartPopupMinigame(int GameID)
    {
        GameObject appInstance = MinigamesManager.Instance.StartMinigame(GameID, 0, Vector2.zero).gameObject;

        GameObject newWindow = Instantiate(windowPrefab, windowParent != null ? windowParent : null);

        appInstance.transform.SetParent(newWindow.transform);

        appInstance.transform.localPosition = Vector3.zero;

        activeWindows.Add(newWindow);
    }
}
