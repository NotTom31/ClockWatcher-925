using UnityEngine;
public class Interactable : MonoBehaviour
{
    public bool interactableEnabled;
    public bool canInteract = true;

    /// <summary>
    /// Triggers the interaction of the object if the player interacts.
    /// </summary>
    public virtual void Interact()
    {
        if(canInteract)
        {
            interactableEnabled = !interactableEnabled;
        }
    }
}
