using UnityEngine;
public class Interactable : MonoBehaviour
{
    public bool interactableEnabled;

    /// <summary>
    /// Triggers the interaction of the object if the player interacts.
    /// </summary>
    public virtual void Interact()
    {
        interactableEnabled = !interactableEnabled;
    }
}
