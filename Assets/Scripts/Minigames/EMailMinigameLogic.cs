using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EMailMinigameLogic : MinigameLogic
{
    [Header("Data")]
    [SerializeField] private EMailData emailData;

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
    [SerializeField] private GameObject inboxScreen;
    [SerializeField] private GameObject composeScreen;

    private string fullBodyText;
    private string[] bodyWords;
    private int wordsRevealed = 0;
    private bool isRevealing = false;
    private int charTypedSinceLastWordTotal = 0;
    private int charTypedSinceLastWord = 0; // counts characters typed since last reveal
    private int charsPerWord; // how many chars typed before revealing a word
    private bool containsMessage;
    private CanvasGroup inboxCanvasGroup;
    private CanvasGroup composeCanvasGroup;

    private void Start()
    {
        LoadEmail(emailData);
        sendButton.onClick.AddListener(SendEmail);
        composeButton.onClick.AddListener(OpenCompose);
        sendButton.interactable = false;

        // Start reveal when player begins typing
        composeField.onValueChanged.AddListener(OnTypingStarted);

        inboxCanvasGroup = inboxScreen.GetComponent<CanvasGroup>();
        composeCanvasGroup = composeScreen.GetComponent<CanvasGroup>();

        OpenInbox();
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
        charsPerWord = data.charsPerWord;
        containsMessage = data.containsMessage;
        recievedMessageText.text = data.recievedMessageText;

        if (containsMessage)
            composeButtonText.text = "Reply";
        else
            composeButtonText.text = "Compose";

        fullBodyText = data.bodyText;
        bodyWords = fullBodyText.Split(' ');
        bodyText.text = "";
        wordsRevealed = 0;
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
            inboxCanvasGroup.alpha = 1f;
            inboxCanvasGroup.interactable = true;
            inboxCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            inboxCanvasGroup.alpha = 0f;
            inboxCanvasGroup.interactable = false;
            inboxCanvasGroup.blocksRaycasts = false;
        }
    }

    public void OpenCompose()
    {
        ToggleReplyUI(true);
        ToggleInboxUI(false);
    }

    public void OpenInbox()
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
