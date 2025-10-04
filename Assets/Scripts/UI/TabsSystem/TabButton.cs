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
    /// Called when a new tab is created
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

    public void CloseTab()
    {
        AssignedApp.SetActive(false);
        _button.interactable = false;
        _button.onClick.RemoveListener(TabSelected);
        this.gameObject.SetActive(false);
    }

    public void TabSelected()
    {
        AssignedApp.SetActive(true); //change so that its just not visible or something?
    }

    public void TabDeselected()
    {
        AssignedApp.SetActive(false);
    }
}
