using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName ="Tab Data/Areas", fileName = "New Area")]
public class AreaData : ScriptableObject
{
    public string AreaName;
    public List<TabsData> Tabs = new List<TabsData>();
}
