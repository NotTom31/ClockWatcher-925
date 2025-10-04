using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabSelectEventSystemHandler : DynamicEventSystemHandler
{
    private Image _image;
    private TabButton _tabButton;
    private TabSelectManager _tabSelectManager;

    private void Awake()
    {
        _tabSelectManager = GetComponentInParent<TabSelectManager>();
    }

    public override void OnPointerEnter(BaseEventData eventData) { }

    public override void OnPointerExit(BaseEventData eventData) { }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        _image = eventData.selectedObject.GetComponent<Image>();
        _tabButton = eventData.selectedObject.GetComponent<TabButton>();

        if (_tabButton != null)
        {
            _tabSelectManager.TabHeaderText.SetText(_tabButton.TabData.tabName);

            if (_image != null)
            {
                _image.color = Color.white;
            }
        }
        _tabButton.TabSelected();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        if (_tabButton != null)
        {
            _tabSelectManager.TabHeaderText.SetText(_tabButton.TabData.tabName);

            if (_image != null)
            {
                _image.color = _tabButton.ReturnColor;
            }
        }
        _tabButton.TabDeselected();
    }
}
