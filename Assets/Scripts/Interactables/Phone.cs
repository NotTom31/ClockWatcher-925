using UnityEngine;

public class Phone : Interactable
{
    public Light phoneLight;

    private void Start()
    {
        phoneLight = GetComponentInChildren<Light>();
    }
    void Update()
    {
        HandlePhoneLight();
    }
    
    /// <summary>
    /// Toggle the phone light on and off.
    /// </summary>
    private void HandlePhoneLight()
    {
        if(canInteract)
        {
            phoneLight.intensity = 1f;
            canInteract = true;
        }
        else
        {
            phoneLight.intensity = 0;
            canInteract = false;
        }
    }

    public override void Interact()
    {
        if(canInteract)
        {
            base.Interact();
            canInteract = false;
        }
    }

    /// <summary>
    /// Hanldes playing the phone event auido.
    /// </summary>
    public void HandlePlayingSound()
    {
        //TODO Modifiy this function to play call audio and handle events
        //Check to see if the base class is enabled, that means the pl
        if(interactableEnabled)
        {

        }
    }
}
