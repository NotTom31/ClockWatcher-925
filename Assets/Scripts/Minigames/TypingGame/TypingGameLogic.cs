using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TypingGameLogic : MinigameLogic
{
    [SerializeField] List<TypingData> typingDataSOs;
    [SerializeField] TextMeshProUGUI prompt;
    [SerializeField] TextMeshProUGUI wrongText;
    [SerializeField] TMP_InputField replyField;
    [SerializeField] float wrongNotifTime;
    List<string> assignments;
    float assignmentWeight;
    int assignmentIndex;
    Coroutine wrongNotifRoutine = null;

    private void Update()
    {
        if (replyField.isFocused)
        {
            ComputerManager.instance.isTyping2 = true;
        }
        else
        {
            ComputerManager.instance.isTyping2 = false;
        }
    }

    public override float EvaluateScore()
    {
        return assignmentIndex * assignmentWeight;
    }

    public override void InstantiateGame(int difficulty = 0)
    {
        if (difficulty >= typingDataSOs.Count)
        {
            Debug.LogError("A TypingData scriptable object is not assigned for this difficulty: " + difficulty);
        }
        assignments = typingDataSOs[difficulty].stringList;
        assignmentWeight = 100f / assignments.Count;
        replyField.onEndEdit.AddListener(OnEndEdit);
        prompt.text = assignments[0];
    }

    private void OnEndEdit(string text)
    {
        if (Keyboard.current != null && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame))
            ProcessString(text);
    }

    private void NextAssignment()
    {
        assignmentIndex++;
        if (assignmentIndex == assignments.Count)
        {
            MinigamesManager.Instance.EndMinigame(this);
            return;
        }
        prompt.text = assignments[assignmentIndex];
        ClearText();
    }

    public void ProcessString(string input)
    {
        // clear "wrong" notif
        if (wrongNotifRoutine != null)
            StopCoroutine(wrongNotifRoutine);
        wrongText.gameObject.SetActive(false);

        // check if correct transcription
        if (input == assignments[assignmentIndex])
        {
            NextAssignment();
        }
        else
        {
            Debug.Log("Input is: " + input + ", and assignment is " + assignments[assignmentIndex]);
            wrongNotifRoutine = StartCoroutine(DoWrongNotif());
        }

        // Make sure field is active
        replyField.Select();
        replyField.ActivateInputField();

        // Move caret to end of text, without highlighting
        replyField.caretPosition = replyField.text.Length;
        replyField.selectionAnchorPosition = replyField.caretPosition;
        replyField.selectionFocusPosition = replyField.caretPosition;
    }

    public void ClearText()
    {
        replyField.text = string.Empty;
    }

    public void Submit()
    {
        ProcessString(replyField.text);
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
