using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool onComputer;
    public bool jumpScaring;

    public static PlayerManager instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        onComputer = false;
    }
}
