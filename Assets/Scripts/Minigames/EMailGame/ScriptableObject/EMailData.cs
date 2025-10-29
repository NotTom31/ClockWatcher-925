using UnityEngine;

[CreateAssetMenu(menuName = "EMail Data/Contents", fileName = "New EMail")]

public class EMailData : ScriptableObject
{
    public string subjectText;
    public string toText;
    public string fromText;
    public int charsPerWord;
    public bool containsMessage;
    public bool canReply;
    [TextArea(3, 10)]
    public string recievedMessageText;

    public string replySubjectText;
    public string replyToText;
    public string replyFromText;

    [TextArea(3, 10)]
    public string bodyText;
}
