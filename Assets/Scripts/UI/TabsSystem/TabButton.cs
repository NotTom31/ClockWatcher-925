using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; //Might not need

public class TabButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tabNameText;

    public TabData TabData { get; set; }

    public GameObject AssignedApp;

    private Button _button;

    private Image _image;

    public Color ReturnColor { get; set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        ReturnColor = Color.grey;
    }

    /// <summary>
    /// Method for setting up initial tabs in the scene.
    /// </summary>
    /// <param name="tab">Scriptable object containing each tabs data such as the app prefab and tab name</param>
    /// <param name="isInteractable">Determines if the tab button can be interacted with. Usually should be set to true</param>
    public void Setup(TabData tab, bool isInteractable)
    {
        TabData = tab;
        _tabNameText.SetText(TabData.tabName);

        _button.interactable = isInteractable;

        if (isInteractable)
        {
            _button.onClick.AddListener(TabSelected);
            ReturnColor = Color.grey;
            _image.color = ReturnColor;
        }
        else
        {
            ReturnColor = Color.grey;
            _image.color = ReturnColor;
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Open tab is a method for opening tabs after the initial scene setup has already happened. Tabs that are created during initial scene setup are made by Setup
    /// </summary>
    public void OpenTab()
    {
        this.gameObject.SetActive(true);
        _button.interactable = true;
        _button.onClick.AddListener(TabSelected);
        ReturnColor = Color.grey;
        _image.color = ReturnColor;
        AssignedApp.transform.SetAsLastSibling();
    }

    /// <summary>
    /// Close tab is called when a tab is shut, not when switching tabs
    /// </summary>
    public void CloseTab()
    {
        AssignedApp.SetActive(false);
        _button.interactable = false;
        _button.onClick.RemoveListener(TabSelected);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the tabs corresponding app as visible and interactable when selected
    /// </summary>
    public void TabSelected()
    {
        CanvasGroup canvasGroup = AssignedApp.GetComponent<CanvasGroup>();

        // Make app visible and interactable
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Sets the tabs corresponding app as not visible and non-interactable when the tab button is deselected.
    /// </summary>
    public void TabDeselected()
    {
        CanvasGroup canvasGroup = AssignedApp.GetComponent<CanvasGroup>();

        // Make app invisible and non-interactable
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
