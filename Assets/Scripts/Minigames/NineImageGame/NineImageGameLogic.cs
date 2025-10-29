using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NineImageGameLogic : MinigameLogic
{
    [SerializeField] List<GridSequence> sequenceByDifficulty;
    [SerializeField] CellImage[] cells;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI wrongText;
    [SerializeField] float wrongNotifTime;
    List<GridProfile> profiles;
    int profileIndex = 0;
    Coroutine wrongNotifRoutine = null;

    public override float EvaluateScore()
    {
        if (profiles == null ||  profiles.Count == 0)
        {
            Debug.LogError("Tried to score NineImageGame with no GridProfile data");
            return 0f;
        }
        return 100f / profiles.Count * profileIndex;
    }

    public override void InstantiateGame(int difficulty = 0)
    {
        profiles = sequenceByDifficulty[difficulty].profiles;
        InitGrid(profiles[0]);
    }

    // Visually populates the grid with the needed images
    private void InitGrid(GridProfile profile)
    {
        if (cells.Length != 9 || profile.sprites.Length != 9 || profile.solution.Length != 9)
        {
            Debug.LogError("Cell Image list or GridProfile scriptable object contains incorrect dataset. Must be length 9");
            return;
        }
        for (int ii = 0; ii < 9; ii++)
        {
            cells[ii].GetComponent<Image>().sprite = profile.sprites[ii];
        }
        DeactivateCells();
        messageText.text = profile.message;
    }

    private void DeactivateCells()
    {
        foreach (CellImage cell in cells)
            cell.SetActivated(false);
    }

    public void SubmitSolution()
    {
        // check if the player has selected the appropriate cells
        bool correct = true;
        for (int ii = 0; ii < 9; ii++)
            if (cells[ii].IsActivated() != profiles[profileIndex].solution[ii])
                correct = false;
        
        if (correct)
        {
            // stop showing "wrong" notif if necessary
            if (wrongNotifRoutine != null)
            {
                StopCoroutine(wrongNotifRoutine);
                wrongNotifRoutine = null;
                wrongText.gameObject.SetActive(false);
            }

            // move to next grid
            profileIndex++;

            // end this game if there are no more grids
            if (profileIndex == profiles.Count)
            {
                Debug.Log("finished NineImageGame");
                MinigamesManager.Instance.EndMinigame(this);
            }
            // or initialize the next grid
            else
                InitGrid(profiles[profileIndex]);
        }
        else
        {
            DeactivateCells();
            wrongNotifRoutine = StartCoroutine(DoWrongNotif());
        }
    }

    // Flash a message on the screen that the solution was incorrect, and keep it for a fixed amount of time
    private IEnumerator DoWrongNotif()
    {
        wrongText.gameObject.SetActive(true);
        yield return new WaitForSeconds(wrongNotifTime);
        wrongText.gameObject.SetActive(false);
        wrongNotifRoutine = null;
    }
}
