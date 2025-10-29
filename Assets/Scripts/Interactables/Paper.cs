using UnityEngine;
using FMODUnity;

public class Paper : Interactable
{
    public GameObject paper;

    public static Paper instance;

    public int currentPaperCount = 0;
    public int maxPaperCount = 1;

    [Header("Timer Settings")]
    public float incrementDelay = 7f; // seconds
    private bool isIncrementing = false;
    private float timer = 0f;

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
    }

    private void Start()
    {
        uiText = "Print more paper";
    }

    public override void Interact()
    {
        if (PlayerManager.instance.holdingObject == false)
        {
            if (currentPaperCount > 0)
            {
                // Set the player to holding an object
                PlayerManager.instance.holdingObject = true;

                // Spawn a paper ball
                PlayerManager.instance.objectInHand = Instantiate(paper, CameraManager.instance.objectHolder.position, CameraManager.instance.objectHolder.rotation) as GameObject;
                PlayerManager.instance.objectInHand.transform.parent = CameraManager.instance.objectHolder.transform;

                RuntimeManager.PlayOneShot(FMODEvents.instance.ballingPaper, this.gameObject.transform.position);

                currentPaperCount--;

                if (currentPaperCount == 0)
                {
                    uiText = "Print more paper";
                }
            }
        }
    }

    public void IncrementPaper()
    {
        // Don't start incrementing if already at max or timer is running
        if (currentPaperCount >= maxPaperCount || isIncrementing)
            return;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.printer, this.transform.position);
        isIncrementing = true;
        timer = 0f; // reset timer
        uiText = "Printing paper...";
    }

    private void Update()
    {
        // Handle the timer for paper increment
        if (isIncrementing)
        {
            timer += Time.deltaTime;
            if (timer >= incrementDelay)
            {
                currentPaperCount++;
                uiText = "E to Grab Paper";
                isIncrementing = false;
            }
        }
    }
}
