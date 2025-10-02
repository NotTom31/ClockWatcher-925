using UnityEngine;
using FMOD.Studio;
using FMODUnity;

////Throw away script for testing
public class PlayerMovement : MonoBehaviour, IDataPersistance
{
    Transform cameraObeject;
    InputManager inputManager;
    Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Stats")]
    [SerializeField]
    float rotationSpeed = 10;

    //audio 
    [SerializeField]
    private EventInstance playerFootsteps;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        myTransform = transform;

        cameraObeject = Camera.main.transform;

        playerFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.playerFootsteps);
        playerFootsteps.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));

    }

    public void Update()
    {
        inputManager.TickInput(Time.deltaTime);

        moveDirection = cameraObeject.forward * inputManager.vertical;
        moveDirection += cameraObeject.right * inputManager.horizontal;
        moveDirection.Normalize();

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection,normalVector);
        rigidbody.linearVelocity = projectedVelocity;


        HandleSound();
    }

    public void FixedUpdate()
    {
        HandleRotation(Time.deltaTime);
    }

    #region Movement
    Vector3 normalVector;

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputManager.moveAmount;

        targetDir = cameraObeject.transform.forward * inputManager.vertical;
        targetDir += cameraObeject.right * inputManager.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if(targetDir == Vector3.zero)
            targetDir = myTransform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation; 
    }

    #endregion

    private void HandleSound()
    {
        if (rigidbody.linearVelocity.x != 0)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if(playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            } 
        }
        else
        {
            playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }
    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
