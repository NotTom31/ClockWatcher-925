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
    [SerializeField] private Button replyButton;
    [SerializeField] private Button sendButton;

    private bool isReplying = false;

    void Start()
    {
        LoadEmail(emailData);
        replyButton.onClick.AddListener(StartReply);
        sendButton.onClick.AddListener(SendEmail);
        sendButton.interactable = false;
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
        bodyText.text = data.bodyText;
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

    void StartReply()
    {
        isReplying = true;
        replyButton.interactable = false;
        sendButton.interactable = true;
    }

    void SendEmail()
    {
        isReplying = false;
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
