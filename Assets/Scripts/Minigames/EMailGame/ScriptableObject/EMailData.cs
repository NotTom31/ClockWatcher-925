using UnityEngine;

[CreateAssetMenu(menuName = "EMail Data/Contents", fileName = "New EMail")]

public class EMailData : ScriptableObject
{
    public string subjectText;
    public string toText;
    public string fromText;
    [TextArea(3, 10)]
    public string bodyText;
}
