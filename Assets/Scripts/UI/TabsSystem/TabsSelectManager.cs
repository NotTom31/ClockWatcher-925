using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class TabsSelectManager : MonoBehaviour
{
    public Transform TabParent;
    public GameObject TabButtonPrefab;
    public TextMeshProUGUI AreaHeaderText;
    public TextMeshProUGUI TabHeaderText;
    public AreaData CurrentArea;

    public List<string> OpenedTabIDs = new List<string>(); //Tutorial uses HashSet, but in our case we want to allow duplicates

    private Camera _camera;

    private List<GameObject> _buttonObjects = new List<GameObject>();
    private Dictionary<GameObject, Vector3> _buttonLocations = new Dictionary<GameObject, Vector3>();

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        AssignAreaText();
        LoadOpenTabs();
        CreateTabButtons();
    }

    public void AssignAreaText()
    {
        AreaHeaderText.SetText(CurrentArea.AreaName);
    }

    private void LoadOpenTabs()
    {
        foreach(var tab in CurrentArea.Tabs)
        {
            if (tab.ISUnlockedByDefault)
            {
                OpenedTabIDs.Add(tab.TabID);
            }
        }
    }

    private void CreateTabButtons()
    {
        for (int i = 0; i < CurrentArea.Tabs.Count; i++)
        {
            GameObject buttonGO = Instantiate(TabButtonPrefab, TabParent);
            _buttonObjects.Add(buttonGO);

            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();

            buttonGO.name = CurrentArea.Tabs[i].name; //Might not work?
            CurrentArea.Tabs[i].tabButtonObj = buttonGO;

            TabsButton tabsButton = buttonGO.GetComponent<TabsButton>();
            tabsButton.Setup(CurrentArea.Tabs[i], OpenedTabIDs.Contains(CurrentArea.Tabs[i].TabID));
        }
    }
}
