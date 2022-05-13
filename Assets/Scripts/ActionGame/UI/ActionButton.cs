using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float thinkTime = 0.05f; //判断长按还是短按的时间

    private bool pressed;
    private bool isStart = false;
    private float beginTime = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        isStart = true;
        beginTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        isStart = false;
    }

    public bool GetActionButton()
    {
        return pressed;
    }

    public bool GetActionButtonDown()
    {
        if (pressed)
        {
            pressed = false;
            return true;
        }
        return false;
    }

    public bool IsShortPress()
    {
        if (Time.time - beginTime < thinkTime && isStart == false)
        {
            beginTime = 0;
            return true;
        }
        return false;
    }

    public bool IsLongPress()
    {
        if (Time.time - beginTime > thinkTime && isStart == true)
        {
            isStart = false;
            return true;
        }
        return false;
    }

    public bool GetPressTimePercentage(ref float percentage)
    {
        if (isStart && (Time.time - beginTime) <= thinkTime)
        {
            return false;
        }
        if (isStart && (Time.time - beginTime) > thinkTime)
        {
            isStart = false;
            percentage = 1;
            return true;
        }
        percentage = (Time.time - beginTime) / thinkTime;
        return true;
    }
}
