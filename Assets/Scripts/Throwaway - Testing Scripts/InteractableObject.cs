using FMODUnity;
using UnityEngine;


//Throw away script for testing
[RequireComponent(typeof(StudioEventEmitter))]
public class InteractableObject : MonoBehaviour
{
    private bool interacted = false;

    private StudioEventEmitter emitter;

    private void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.interactableIdle, this.gameObject);
        emitter.Play();
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Entered!");

        if(!interacted)
        {
            Interact();
        }
    }

    private void Interact()
    {
        interacted = true;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.interacted, this.transform.position);
        emitter.Stop();
    }
}
