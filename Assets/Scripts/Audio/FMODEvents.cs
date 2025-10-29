using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    //Set the reference to the FMOD studio file per sound file.
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: Header("Interace SFX")]
    [field: SerializeField] public EventReference paperImpact { get; private set; }
    [field: SerializeField] public EventReference ballingPaper { get; private set; }
    [field: SerializeField] public EventReference interacted { get; private set; }
    [field: SerializeField] public EventReference interactableIdle { get; private set; }
    [field: SerializeField] public EventReference printer { get; private set; }
    [field: SerializeField] public EventReference lightSwitch { get; private set; }
    [field: SerializeField] public EventReference throwPaper { get; private set; }

    [field: Header("Computer SFX")]
    [field: SerializeField] public EventReference click { get; private set; }
    [field: SerializeField] public EventReference hover { get; private set; }
    [field: SerializeField] public EventReference computerOpen { get; private set; }
    [field: SerializeField] public EventReference computerClose { get; private set; }
    [field: SerializeField] public EventReference keyEntered { get; private set; }
    [field: SerializeField] public EventReference keySpamAll { get; private set; }
    [field: SerializeField] public EventReference keySpamShort { get; private set; }
    [field: SerializeField] public EventReference keySpamLong { get; private set; }
    [field: SerializeField] public EventReference negFeedback { get; private set; }
    [field: SerializeField] public EventReference posFeedback { get; private set; }
    [field: SerializeField] public EventReference pos2Feedback { get; private set; }
    [field: SerializeField] public EventReference fileOpen { get; private set; }
    [field: SerializeField] public EventReference fileClose { get; private set; }
    [field: SerializeField] public EventReference emailSent { get; private set; }
    [field: SerializeField] public EventReference emailRecieved { get; private set; }
    [field: SerializeField] public EventReference waterPlant { get; private set; }
    [field: SerializeField] public EventReference trashFile { get; private set; }

    [field: Header("Monsters SFX")]
    [field: SerializeField] public EventReference crawlerClose { get; private set; }
    [field: SerializeField] public EventReference crawlerFar { get; private set; }
    [field: SerializeField] public EventReference crawlerImpact { get; private set; }
    [field: SerializeField] public EventReference wagsIdle { get; private set; }
    [field: SerializeField] public EventReference wagsRage { get; private set; }
    [field: SerializeField] public EventReference wagsStep { get; private set; }
    [field: SerializeField] public EventReference sting1 { get; private set; }
    [field: SerializeField] public EventReference sting2 { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
