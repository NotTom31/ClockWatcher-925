using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool onComputer;
    public bool jumpScaring;

    public bool holdingObject;
    public GameObject objectInHand;

    public float ballOfPaperForwardForce = 15f;
    public float ballOfPaperUpwardForce = 1.5f;

    public static PlayerManager instance;


    private void Awake()
    {
        if(instance == null)
            instance = this;

        holdingObject = false;
    }

    private void Start()
    {
        onComputer = false;
    }

    /// <summary>
    /// Handles shooting the paper ball.
    /// </summary>
    public void shootPaper()
    {
        holdingObject = false;
        objectInHand.transform.parent = null;
        objectInHand.AddComponent<Rigidbody>();
        objectInHand.GetComponent<Rigidbody>().AddForce(CameraManager.instance.transform.forward * ballOfPaperForwardForce, ForceMode.Impulse);
        objectInHand.GetComponent<Rigidbody>().AddForce(CameraManager.instance.transform.up * ballOfPaperUpwardForce, ForceMode.Impulse);
        objectInHand.GetComponent<PaperBallHandler>().StartCoroutine(objectInHand.GetComponent<PaperBallHandler>().SelfDestruct());
        objectInHand = null;
    }
}
