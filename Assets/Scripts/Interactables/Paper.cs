
using UnityEngine;
using FMODUnity;
public class Paper : Interactable
{
    public GameObject paper;

    public static Paper instance;

    public int currentPaperCount = 0;

    public int maxPaperCount = 1;

    private void Start()
    {
        uiText = "print more paper";
    }

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

    public override void Interact()
    {
        if(PlayerManager.instance.holdingObject == false)
        {
            if(currentPaperCount > 0)
            {
                //Set the player to holding an object
                PlayerManager.instance.holdingObject = true;
                //Spawn a paper ball
                PlayerManager.instance.objectInHand = (Instantiate(paper, CameraManager.instance.objectHolder.position, CameraManager.instance.objectHolder.rotation) as GameObject);
                //Set ball as child object of the player holder.
                PlayerManager.instance.objectInHand.transform.parent = CameraManager.instance.objectHolder.transform;

                RuntimeManager.PlayOneShot(FMODEvents.instance.ballingPaper, this.gameObject.transform.position);

                currentPaperCount--;

                if(currentPaperCount == 0)
                {
                    uiText = "print more paper";
                }
            }
        }
    }

    public void IncrementPaper()
    {
        if(currentPaperCount == maxPaperCount)
        {
            return;
        }        
        currentPaperCount++;

        uiText = "Grab Paper";
    }
}   
