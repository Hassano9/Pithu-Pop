
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform bg;
    private RectTransform handle;
    private Vector2 inputVector;

    void Start()
    {
        bg = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>(); // assumes child is handle
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bg, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / (bg.sizeDelta.x / 2));
            pos.y = (pos.y / (bg.sizeDelta.y / 2));

            inputVector = new Vector2(pos.x, pos.y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // move handle
            handle.anchoredPosition = new Vector2(inputVector.x * (bg.sizeDelta.x / 2),
                                                  inputVector.y * (bg.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero; // reset handle
    }

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;
}

