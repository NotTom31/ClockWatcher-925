using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Parameter Change")]
    [SerializeField] private string parameterName;
    [SerializeField] private float parameter;

    /// <summary>
    /// Triggers the change of a FMOD parameter to a new value
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<InputManager>())
        {
            AudioManager.instance.SetAmbienceParameter(parameterName, parameter);
        }
    }
}
