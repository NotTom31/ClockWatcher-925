using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class ComputerManager : Interactable
{
    public static ComputerManager instance { get; private set; }
    public Transform cameraPosition;
    private Camera mainCamera;
    private bool browserEnabled = true;
    private bool emailEnabled = false;
    public bool isTyping = false;
    public bool isTyping2 = false;

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D grabCursor;
    [SerializeField] private Texture2D dragCursor;
    [SerializeField] private Vector2 normalHotspot = Vector2.zero;
    [SerializeField] private Vector2 clickHotspot = Vector2.zero;
    [SerializeField] private Vector2 grabHotspot = Vector2.zero;
    [SerializeField] private Vector2 dragHotspot = Vector2.zero;

    [Header("Window Settings")]
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private Transform windowParent;

    [Header("Computer Canavs")]
    [SerializeField] private Canvas computerCanvas;
    [SerializeField] private GameObject browser;
    [SerializeField] private GameObject email;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI emailCountText;

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

        uiText = "E to get on computer";
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SetCanvasCamera();
    }

    public void UpdateTimeUI(string time)
    {
        timeText.text = time;
    }

    public void UpdateEmailCountUI(string count)
    {
        emailCountText.text = count;
    }

    public override void Interact()
    {
        if (isTyping || isTyping2)
            return;

        //toggles the player being on the computer
        PlayerManager.instance.onComputer = !PlayerManager.instance.onComputer;

        if (PlayerManager.instance.onComputer)
        {
            //unlocks the mouse and make it visable and set the camera target position to the computer
            CameraManager.instance.SetMouseLockState(false);
            CameraManager.instance.targetTransform = cameraPosition;
            CameraManager.instance.cameraLock = true;
            //StartPopupMinigame(3);
        }
        else
        {
            //Locks the mouse and make it invisable and set the camera target position to the center of the "room"
            CameraManager.instance.SetMouseLockState(true);
            CameraManager.instance.targetTransform = CameraManager.instance.orientation;
            CameraManager.instance.cameraLock = false;

        }
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, normalHotspot, CursorMode.Auto);
    }

    public void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, clickHotspot, CursorMode.Auto);
    }

    public void SetGrabCursor()
    {
        Cursor.SetCursor(grabCursor, grabHotspot, CursorMode.Auto);
    }

    public void SetDragCursor()
    {
        Cursor.SetCursor(dragCursor, dragHotspot, CursorMode.Auto);
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

    public void ToggleBrowser()
    {
        CanvasGroup canvasGroup = browser.GetComponent<CanvasGroup>();

        if (browserEnabled)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.fileClose, this.transform.position);
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            browserEnabled = false;
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.fileOpen, this.transform.position);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            browserEnabled = true;
        }
    }

    public void ToggleEmail()
    {
        CanvasGroup canvasGroup = email.GetComponent<CanvasGroup>();

        if (emailEnabled)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.fileClose, this.transform.position);
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            emailEnabled = false;
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.fileOpen, this.transform.position);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            emailEnabled = true;
        }
    }

    public void PrintPaper()
    {
        Paper.instance.IncrementPaper();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Every time a scene loads, reassign the camera
        SetCanvasCamera();
    }

    private void SetCanvasCamera()
    {
        // If no camera was assigned manually, grab the scene's main camera
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (computerCanvas == null)
        {
            Debug.LogWarning("ComputerManager: No canvas assigned to set the camera on.");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("ComputerManager: No camera found in the scene.");
            return;
        }

        // Assign the camera to the canvas
        computerCanvas.worldCamera = mainCamera;
        Debug.Log($"ComputerManager: Set {computerCanvas.name}'s camera to {mainCamera.name}");
    }
}
