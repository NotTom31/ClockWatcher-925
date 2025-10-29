using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CellImage : MonoBehaviour, IPointerClickHandler
{
    bool activated;
    Coroutine swellRoutine = null;
    const float SWELL_RATE = 0.6f;
    const float ACTIVATE_SCALE = 0.92f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    public void Toggle()
    {
        SetActivated(!activated);
    }

    public bool IsActivated()
    {
        return activated;
    }

    public void SetActivated(bool b)
    {
        activated = b;
        if (swellRoutine != null)
            StopCoroutine(swellRoutine);
        swellRoutine = StartCoroutine(DoSwell(!activated));
    }

    private IEnumerator DoSwell(bool up)
    {
        float rate = SWELL_RATE;
        float goal = 1f;
        float scl;
        if (!up)
        {
            rate *= -1f;
            goal = ACTIVATE_SCALE;
        }
        while (up && transform.localScale.x < goal || !up && transform.localScale.x > goal)
        {
            scl = transform.localScale.x + rate * Time.deltaTime;
            transform.localScale = new Vector3(scl, scl, scl);
            yield return null;
        }
        transform.localScale = new Vector3(goal, goal, goal);
        swellRoutine = null;
    }
}
