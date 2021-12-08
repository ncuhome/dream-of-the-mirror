using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pressed; //等价与Input.GetKey()

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
}
