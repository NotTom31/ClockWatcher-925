using UnityEngine;
using FMODUnity;

public class Lightswitch : Interactable
{
    private StudioEventEmitter emitter;

    [SerializeField] private float lightTimer = 10;
    [SerializeField] private float currentLightTimer;

    private new Light light;

    void Start()
    {
        //Set the current timer to the max time limit.
        currentLightTimer = lightTimer;

        light = GetComponentInChildren<Light>();

        uiText = "E to turn on light";
    }

    public override void Interact()
    {
        base.Interact();
    }

    void Update()
    {
        HandleLight();
    }

    /// <summary>
    /// Enables or disables the light
    /// </summary>
    public void HandleLight()
    {
        //Check to see if the item has been interacted.
        if (interactableEnabled)
        {
            //Check to see if the light needs to turn off, reset bools and timer if so
            currentLightTimer -= Time.deltaTime;

            if (currentLightTimer <= 0)
            {
                currentLightTimer = 0;
                light.enabled = false;
                interactableEnabled = false;
                uiText = "E to turn on light";
            }
            else
            {
                light.enabled = true;
                uiText = "E to turn off light";
            }
        }
        else
        {
            //If the item has not been interacted, turn it off.
            light.enabled = false;
            currentLightTimer = lightTimer;
            uiText = "E to turn on light";
        }
    }
}
