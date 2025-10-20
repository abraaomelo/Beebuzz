using UnityEngine;
using UnityEngine.EventSystems;

public class TouchJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;

    [HideInInspector]
    public Vector2 inputVector;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out pos);

        pos.x = (pos.x / background.sizeDelta.x) * 2f;
        pos.y = (pos.y / background.sizeDelta.y) * 2f;

        inputVector = new Vector2(pos.x, pos.y);
        inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

        // Move o handle
        handle.anchoredPosition = new Vector2(
            inputVector.x * (background.sizeDelta.x / 2.5f),
            inputVector.y * (background.sizeDelta.y / 2.5f)
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetInput() => inputVector;
}
