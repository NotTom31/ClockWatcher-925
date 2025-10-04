using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; //Might not need

public class TabsButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tabNameText;

    public TabsData TabsData { get; set; }

    private Button _button;

    private Image _image;

    public Color ReturnColor { get; set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        ReturnColor = Color.grey;
    }

    public void Setup(TabsData tab, bool isInteractable)
    {
        TabsData = tab;
        _tabNameText.SetText(TabsData.tabName);

        _button.interactable = isInteractable;

        if (isInteractable)
        {
            _button.onClick.AddListener(OpenTab);
            ReturnColor = Color.white;
            _image.color = ReturnColor;
        }
        else
        {
            ReturnColor = Color.grey;
            _image.color = ReturnColor;
        }
    }

    public void Unlock()
    {
        _button.interactable = true;
        _button.onClick.AddListener(OpenTab);
        ReturnColor = Color.white;
        _image.color = ReturnColor;
    }

    private void OpenTab() //This might need to be changed entirely
    {
        SceneManager.LoadScene(TabsData.Scene);
    }
}
