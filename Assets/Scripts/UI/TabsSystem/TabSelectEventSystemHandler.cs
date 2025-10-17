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

        // Cache the new selection's image and tab button (if any)
        Image newImage = eventData.selectedObject.GetComponent<Image>();
        TabButton newTabButton = eventData.selectedObject.GetComponent<TabButton>() ?? eventData.selectedObject.GetComponentInParent<TabButton>();

        // If we're selecting a TabButton, handle switching from previous tab to new tab
        if (newTabButton != null)
        {
            // If there was a previous tab that is different and in the same manager, deselect it
            if (_tabButton != null && _tabButton != newTabButton &&
                _tabButton.GetComponentInParent<TabSelectManager>() == _tabSelectManager)
            {
                // revert previous tab visuals (if we still have its image stored)
                if (_image != null)
                {
                    _image.color = _tabButton.ReturnColor;
                }

                _tabButton.TabDeselected();
            }

            // Update header and visuals for the newly selected tab
            _tabSelectManager.TabHeaderText.SetText(newTabButton.TabData.tabName);
            if (newImage != null)
                newImage.color = Color.white;

            _tabButton = newTabButton;
            _image = newImage;

            // call selected on the new tab
            _tabButton.TabSelected();
        }
        else
        {
            _image = newImage;
        }
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
        //_tabButton.TabDeselected();
    }
}
