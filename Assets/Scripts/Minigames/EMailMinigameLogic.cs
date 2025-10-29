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

    public override float EvaluateScore()
    {
        return 0;
    }

    public override void InstantiateGame(int difficulty = 0)
    {
        //throw new System.NotImplementedException();
    }
}
