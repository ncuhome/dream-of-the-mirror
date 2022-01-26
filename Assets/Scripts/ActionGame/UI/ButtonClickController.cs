using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pressed; //等价与Input.GetKey()

    public float thinkTime = 0.05f; //判断长按还是短按的时间
    public bool isStart = false;
    public float lastTime = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        LongPress(true);
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        if (isStart)
        {
            LongPress(false);
        }
    }

    public void LongPress(bool bStart)
    {
        isStart = bStart;
        lastTime = Time.time;
    }
}
