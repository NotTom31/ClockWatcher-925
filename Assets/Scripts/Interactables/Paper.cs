
using UnityEngine;
using FMODUnity;
public class Paper : Interactable
{
    public GameObject paper;
    public override void Interact()
    {
        if(PlayerManager.instance.holdingObject == false)
        {
            //Set the player to holding an object
            PlayerManager.instance.holdingObject = true;
            //Spawn a paper ball
            PlayerManager.instance.objectInHand = (Instantiate(paper, CameraManager.instance.objectHolder.position, CameraManager.instance.objectHolder.rotation) as GameObject);
            //Set ball as child object of the player holder.
            PlayerManager.instance.objectInHand.transform.parent = CameraManager.instance.objectHolder.transform;

            RuntimeManager.PlayOneShot(FMODEvents.instance.ballingPaper, this.gameObject.transform.position);

        }
    }
}   
