using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Pressed; //等价与Input.GetKey()

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
