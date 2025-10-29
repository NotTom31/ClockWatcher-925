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
    [SerializeField] private TMP_Text bodyText;

    [SerializeField] private TMP_InputField replyField;
    [SerializeField] private Button sendButton;

    private string fullBodyText;
    private string[] bodyWords;
    private int wordsRevealed = 0;
    private bool isRevealing = false;
    private int charTypedSinceLastWordTotal = 0;
    private int charTypedSinceLastWord = 0; // counts characters typed since last reveal
    [SerializeField] private int charsPerWord = 3; // how many chars typed before revealing a word

    void Start()
    {
        LoadEmail(emailData);
        sendButton.onClick.AddListener(SendEmail);
        sendButton.interactable = false;

        // Start reveal when player begins typing
        replyField.onValueChanged.AddListener(OnTypingStarted);
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
            sendButton.interactable = true;
        }

        if (wordsRevealed < bodyWords.Length)
        {
            // Count characters typed since last word
            charTypedSinceLastWord += text.Length - charTypedSinceLastWordTotal;
            charTypedSinceLastWordTotal = text.Length;

            if (charTypedSinceLastWord >= charsPerWord)
            {
                // Reveal a word
                bodyText.text += (wordsRevealed > 0 ? " " : "") + bodyWords[wordsRevealed];
                wordsRevealed++;
                charTypedSinceLastWord = 0;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.keySpamShort, this.transform.position);
            }
        }
    }

    void Update()
    {
        if (replyField.isFocused)
        {
            ComputerManager.instance.isTyping = true;
        }
        else
        {
            ComputerManager.instance.isTyping = false;
        }
    }

    void SendEmail()
    {
        sendButton.interactable = false;
        Debug.Log("Sent reply: " + replyField.text);
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
