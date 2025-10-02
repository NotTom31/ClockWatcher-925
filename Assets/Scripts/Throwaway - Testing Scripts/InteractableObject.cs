using FMODUnity;
using UnityEngine;

//Throw away script for testing
[RequireComponent(typeof(StudioEventEmitter))]
public class InteractableObject : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;

    //Used in the Unity GUI to right click and generate a unique ID for saving/loading the game.
    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public GameObject model;

    private bool interacted = false;

    private StudioEventEmitter emitter;

    private void Start()
    {
        //Sets the audio emitter to the interact idle sound.
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.interactableIdle, this.gameObject);

        //if the object has not been interacted in the save file, play the audio sound.
        if (!interacted)
        {
            emitter.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!interacted)
        {
            Interact();
        }
    }

    /// <summary>
    /// Handles the interaction with the player and the object.
    /// </summary>
    private void Interact()
    {
        interacted = true;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.interacted, this.transform.position);
        UIManager.instance.interactionCount++;
        emitter.Stop();
        model.gameObject.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        data.interactionsDone.TryGetValue(id, out interacted);
        if (interacted)
        {
            model.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.interactionsDone.ContainsKey(id))
        {
            data.interactionsDone.Remove(id);
        }

        data.interactionsDone.Add(id, interacted);
    }
}
