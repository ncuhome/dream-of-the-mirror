using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //长按与短按的判定：
    // if (!isJump)
    // {
    //     if (!jumpBtn.isStart)
    //     {
    //         rb.AddForce(Vector2.up * shortJumpForce, ForceMode2D.Impulse);
    //         isJump = true;
    //     }
    //     else if (Time.time - jumpBtn.lastTime > jumpBtn.thinkTime)
    //     {
    //         jumpBtn.isStart = false;
    //         rb.AddForce(Vector2.up * longJumpForce, ForceMode2D.Impulse);
    //         isJump = true;
    //     }
    // }
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
