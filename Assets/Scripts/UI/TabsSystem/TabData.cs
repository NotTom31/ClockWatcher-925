using UnityEngine;

[CreateAssetMenu(menuName ="Tab Data/Tabs", fileName = "New Tab")]

public class TabData : ScriptableObject
{
    [Header("Tab Stats")]
    public string TabID;
    public int MinigameID;
    [Tooltip("For Starting Tabs")] public bool ISUnlockedByDefault;

    [Header("Optional scene reference")] //May be useful for main menu scene switching, or may go unused
    public SceneField Scene;

    [Header("Tab Display Information")]
    public string tabName;

    [Header("Tab App Prefab")]
    public GameObject app;

    public GameObject tabButtonObj { get; set; }
}
