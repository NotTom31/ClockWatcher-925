using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    float movementSpeed = 5;
    [SerializeField]
    float rotationSpeed = 10;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        myTransform = transform;

        cameraObeject = Camera.main.transform;

    }

    public void Update()
    {
        inputManager.TickInput(Time.deltaTime);

        moveDirection = cameraObeject.forward * inputManager.vertical;
        moveDirection += cameraObeject.right * inputManager.horizontal;
        moveDirection.Normalize();

        float speed = movementSpeed;
        movementSpeed += speed;
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection,normalVector);
        rigidbody.linearVelocity = projectedVelocity;
    }



    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

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

}
