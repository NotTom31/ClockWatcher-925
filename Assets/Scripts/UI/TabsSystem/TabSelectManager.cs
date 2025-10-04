using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class TabSelectManager : MonoBehaviour
{
    public Transform TabParent;
    public Transform AppParent;
    public GameObject TabButtonPrefab;
    public TextMeshProUGUI AreaHeaderText;
    public TextMeshProUGUI TabHeaderText;
    public AreaData CurrentArea;

    public List<string> UnlockedTabIDs = new List<string>(); //Tutorial uses HashSet, but in our case we want to allow duplicates

    private TabSelectEventSystemHandler _eventSystemHandler;

    private Camera _camera;

    private List<GameObject> _buttonObjects = new List<GameObject>();
    private Dictionary<GameObject, Vector3> _buttonLocations = new Dictionary<GameObject, Vector3>();

    private void Awake()
    {
        _camera = Camera.main;
        _eventSystemHandler = GetComponentInChildren<TabSelectEventSystemHandler>(true);
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
                UnlockedTabIDs.Add(tab.TabID);
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

            buttonGO.name = CurrentArea.Tabs[i].name;
            CurrentArea.Tabs[i].tabButtonObj = buttonGO;

            TabButton tabButton = buttonGO.GetComponent<TabButton>();
            tabButton.Setup(CurrentArea.Tabs[i], UnlockedTabIDs.Contains(CurrentArea.Tabs[i].TabID));

            if (tabButton.TabData.app != null)
            {
                GameObject appInstance = Instantiate(tabButton.TabData.app, AppParent);
                appInstance.name = tabButton.TabData.app.name + "_Instance";
                appInstance.SetActive(false);

                tabButton.AssignedApp = appInstance;
            }

            //populate the selectables for the event system
            Selectable selectable = buttonGO.GetComponent<Selectable>();
            _eventSystemHandler.AddSelectable(selectable);
        }

        TabParent.gameObject.SetActive(true);
        _eventSystemHandler.InitSelectables();
        _eventSystemHandler.SetFirstSelected();
    }

    #region Helper Methods

    public void OpenTab(string tabID, TabButton tabButton)
    {
        UnlockedTabIDs.Add(tabID);
        tabButton.transform.SetAsLastSibling();
        tabButton.OpenTab();
    }

    public void CloseTab(string tabID, TabButton tabButton)
    {
        UnlockedTabIDs.Remove(tabID);
        tabButton.CloseTab();
    }

    [ContextMenu("Test Tab Unlocking")]
    public void UnlockTabTwoExample()
    {
        TabButton tabButton = _buttonObjects[1].GetComponent<TabButton>();
        string tabToOpen = tabButton.TabData.TabID;
        OpenTab(tabToOpen, tabButton);
    }

    [ContextMenu("Test Tab Locking")]
    public void LockTabTwoExample()
    {
        TabButton tabButton = _buttonObjects[1].GetComponent<TabButton>();
        string tabToClose = tabButton.TabData.TabID;
        CloseTab(tabToClose, tabButton);
    }

    #endregion
}
