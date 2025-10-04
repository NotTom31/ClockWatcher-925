using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; //Might not need

public class TabButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tabNameText;

    public TabData TabData { get; set; }

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
            _button.onClick.AddListener(OpenTab);
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

    public void Unlock()
    {
        this.gameObject.SetActive(true);
        _button.interactable = true;
        _button.onClick.AddListener(OpenTab);
        ReturnColor = Color.grey;
        _image.color = ReturnColor;
    }

    private void OpenTab() //This might need to be changed entirely
    {
        SceneManager.LoadScene(TabData.Scene);
    }
}
