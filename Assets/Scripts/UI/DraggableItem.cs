using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform parentRect;
    private Canvas parentCanvas;
    private Vector2 dragOffset;

    private void Awake()
    {
        parentRect = transform.parent as RectTransform;
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parentRect == null || parentCanvas == null) return;

        parentRect.SetAsLastSibling();

        // Get local pointer position relative to the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            eventData.position,
            parentCanvas.worldCamera,
            out Vector2 localPointerPos
        );

        dragOffset = parentRect.anchoredPosition - localPointerPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentRect == null || parentCanvas == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            eventData.position,
            parentCanvas.worldCamera,
            out Vector2 localPointerPos
        );

        // Calculate the new position with offset
        Vector2 newPos = localPointerPos + dragOffset;

        // Clamp within the canvas
        RectTransform canvasRect = parentCanvas.transform as RectTransform;
        Vector2 minPosition = canvasRect.rect.min + parentRect.rect.size * parentRect.pivot;
        Vector2 maxPosition = canvasRect.rect.max - parentRect.rect.size * (Vector2.one - parentRect.pivot);

        newPos.x = Mathf.Clamp(newPos.x, minPosition.x, maxPosition.x);
        newPos.y = Mathf.Clamp(newPos.y, minPosition.y, maxPosition.y);

        parentRect.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
