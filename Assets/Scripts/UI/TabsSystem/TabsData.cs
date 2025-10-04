using UnityEngine;

[CreateAssetMenu(menuName ="Tab Data/Tabs", fileName = "New Tab")]

public class TabsData : ScriptableObject
{
    [Header("Level Stats")]
    public string TabID;
    [Tooltip("For Starting Tabs")] public bool ISUnlockedByDefault;
    public SceneField Scene;

    [Header("Level Display Information")]
    public string tabName;

    public GameObject tabButtonObj { get; set; }
}
