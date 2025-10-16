using UnityEngine;

public class ComputerManager : Interactable
{
    public Transform cameraPosition;
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
}
