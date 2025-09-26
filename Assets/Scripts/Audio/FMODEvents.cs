using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: Header("Interace SFX")]
    [field: SerializeField] public EventReference interacted { get; private set; }
    [field: SerializeField] public EventReference interactableIdle { get; private set; }

    public static FMODEvents instance { get; private set;}

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
