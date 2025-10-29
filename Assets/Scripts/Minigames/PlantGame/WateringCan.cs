using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WateringCan : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PlantGameLogic logic;
    [SerializeField] List<GameObject> canFrames; // should be the three pouring frames
    [SerializeField] float frameDelay;
    int pourState = 0; // 0 - 3, where 0 is at rest and 3 is pouring. 1 and 2 are "heading into the adjacent state"
    Coroutine transitionRoutine = null;

    private void SetState(int s)
    {
        if (s < 0 || s > 3)
        {
            Debug.LogError("Tried to set invalid watering can state: " + s);
            return;
        }
        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);
            transitionRoutine = null;
        }

        // Transition out of old state
        switch (pourState)
        {
            case 0:
                canFrames[0].SetActive(false);
                break;
            case 1:
            case 2:
                canFrames[1].SetActive(false);
                break;
            case 3:
                canFrames[2].SetActive(false);
                logic.SetWatering(false);
                break;
        }

        // Transition into new state
        pourState = s;
        switch (pourState)
        {
            case 0:
                canFrames[0].SetActive(true);
                break;
            case 1:
                canFrames[1].SetActive(true);
                transitionRoutine = StartCoroutine(DelayedSetState(0));
                break;
            case 2:
                canFrames[1].SetActive(true);
                transitionRoutine = StartCoroutine(DelayedSetState(3));
                break;
            case 3:
                canFrames[2].SetActive(true);
                logic.SetWatering(true);
                break;
        }
    }

    private IEnumerator DelayedSetState(int s)
    {
        yield return new WaitForSeconds(frameDelay);
        SetState(s);
        transitionRoutine = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (pourState)
        {
            case 0:
                SetState(2);
                break;
            case 1:
                SetState(3);
                break;
            case 2:
                SetState(0);
                break;
            case 3:
                SetState(1);
                break;
        }
    }
}
