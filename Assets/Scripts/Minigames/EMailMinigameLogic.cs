using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EMailMinigameLogic : MinigameLogic
{
    [Header("Data")]
    [SerializeField] private EMailData[] emailData;

    [Header("UI References")]
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private TMP_Text toText;
    [SerializeField] private TMP_Text fromText;
    [SerializeField] private TMP_Text replySubjectText;
    [SerializeField] private TMP_Text replyToText;
    [SerializeField] private TMP_Text replyFromText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private TMP_Text recievedMessageText;

    [SerializeField] private TMP_InputField composeField;
    [SerializeField] private Button sendButton;
    [SerializeField] private Button composeButton;
    [SerializeField] private TMP_Text composeButtonText;
    [SerializeField] private GameObject receivedScreen;
    [SerializeField] private GameObject composeScreen;

    private string fullBodyText;
    private string[] bodyWords;
    private int wordsRevealed = 0;
    private bool isRevealing = false;
    private int charTypedSinceLastWordTotal = 0;
    private int charTypedSinceLastWord = 0; // counts characters typed since last reveal
    private int charsPerWord; // how many chars typed before revealing a word
    private bool containsMessage;
    private bool canReply;
    private CanvasGroup receivedCanvasGroup;
    private CanvasGroup composeCanvasGroup;
    private int emailIndex = 0;

    private void Start()
    {
        LoadEmail(emailData[emailIndex]);
        sendButton.onClick.AddListener(SendEmail);
        composeButton.onClick.AddListener(OpenCompose);
        sendButton.interactable = false;

        // Start reveal when player begins typing
        composeField.onValueChanged.AddListener(OnTypingStarted);

        receivedCanvasGroup = receivedScreen.GetComponent<CanvasGroup>();
        composeCanvasGroup = composeScreen.GetComponent<CanvasGroup>();

        OpenMessage();
    }

    private void LoadEmail(EMailData data)
    {
        if (data == null)
        {
            Debug.LogWarning("No email data assigned!");
            return;
        }

        subjectText.text = data.subjectText;
        toText.text = data.toText;
        fromText.text = data.fromText;
        replySubjectText.text = data.replySubjectText;
        replyToText.text = data.replyToText;
        replyFromText.text = data.replyFromText;
        charsPerWord = data.charsPerWord;
        containsMessage = data.containsMessage;
        canReply = data.canReply;
        recievedMessageText.text = data.recievedMessageText;

        if (containsMessage && canReply)
        {
            composeButtonText.text = "Reply";
        }
        else if (containsMessage && !canReply)
        {
            //composeButton.interactable = false;
            composeButtonText.text = "Mark Read";
        }
        else
        {
            composeButtonText.text = "Compose";
        }


        fullBodyText = data.bodyText;
        bodyWords = fullBodyText.Split(' ');
        bodyText.text = "";
        wordsRevealed = 0;
    }

    public void ResetEmail()
    {
        if (emailData.Length == 0) return;

        // Reset variables
        isRevealing = false;
        wordsRevealed = 0;
        charTypedSinceLastWord = 0;
        charTypedSinceLastWordTotal = 0;

        // Clear UI
        bodyText.text = "";
        composeField.text = "";
        sendButton.interactable = false;

        // Load the new email data
        LoadEmail(emailData[emailIndex + 1]); //temp to keep the game moving

        // Open inbox UI by default
        OpenMessage();
    }


    private void OnTypingStarted(string text)
    {
        if (!isRevealing)
        {
            isRevealing = true;
        }

        if (wordsRevealed < bodyWords.Length)
        {
            charTypedSinceLastWord += text.Length - charTypedSinceLastWordTotal;
            charTypedSinceLastWordTotal = text.Length;

            if (charTypedSinceLastWord >= charsPerWord)
            {
                RevealNextWord();
                charTypedSinceLastWord = 0;
            }
        }
    }

    private void RevealNextWord()
    {
        if (wordsRevealed >= bodyWords.Length) return;

        string nextWord = bodyWords[wordsRevealed];

        bodyText.text += (wordsRevealed > 0 ? " " : "") + nextWord;

        if (nextWord.Contains("\n"))
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.keyEntered, this.transform.position);
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.keySpamShort, this.transform.position);
        }

        wordsRevealed++;

        if (wordsRevealed >= bodyWords.Length)
        {
            sendButton.interactable = true;
        }
    }


    private void Update()
    {
        if (composeField.isFocused)
        {
            ComputerManager.instance.isTyping = true;
        }
        else
        {
            ComputerManager.instance.isTyping = false;
        }
    }

    private void SendEmail()
    {
        sendButton.interactable = false;
        ResetEmail();
        Debug.Log("Sent reply: " + composeField.text);
    }

    private void ToggleReplyUI(bool isEnabled)
    {
        if (isEnabled)
        {
            composeCanvasGroup.alpha = 1f;
            composeCanvasGroup.interactable = true;
            composeCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            composeCanvasGroup.alpha = 0f;
            composeCanvasGroup.interactable = false;
            composeCanvasGroup.blocksRaycasts = false;
        }
    }

    private void ToggleInboxUI(bool isEnabled)
    {
        if (isEnabled)
        {
            receivedCanvasGroup.alpha = 1f;
            receivedCanvasGroup.interactable = true;
            receivedCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            receivedCanvasGroup.alpha = 0f;
            receivedCanvasGroup.interactable = false;
            receivedCanvasGroup.blocksRaycasts = false;
        }
    }

    public void OpenCompose()
    {
        if (containsMessage && !canReply)
        {
            ResetEmail();//temp to keep the game moving
        }
        else
        {
            ToggleReplyUI(true);
            ToggleInboxUI(false);
        }
    }

    public void OpenMessage()
    {
        ToggleReplyUI(false);
        ToggleInboxUI(true);
    }

    public override float EvaluateScore()
    {
        return 0;
    }

    public override void InstantiateGame(int difficulty = 0)
    {
        //throw new System.NotImplementedException();
    }
}
