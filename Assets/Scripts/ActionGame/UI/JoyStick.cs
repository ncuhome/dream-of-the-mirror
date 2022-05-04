using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]

//水平轴控制脚本，通过触摸输入改变轴系统的位置
public class JoyStick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Reference")]
    public RectTransform background;
    public RectTransform knob;
    public float offset = 2f;

    //摇杆的相对位置（即玩家输入值）!
    private Vector2 pointPos;
    private bool dragging = false;

    /// <summary>
    /// 返回摇杆的相对位置（即玩家输入值）
    /// </summary>
    public Vector2 PointPos
    {
        get
        {
            return pointPos;
        }
    }
    public bool isDragged()
    {
        return dragging;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        CalPointPos(ref eventData);
        MovePoint();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        pointPos = new Vector2(0f, 0f);
        knob.transform.position = background.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        background.position = ScreenToWorldPoint(eventData.position);
        knob.position = ScreenToWorldPoint(eventData.position);
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndDrag(eventData);
    }

    private void CalPointPos(ref PointerEventData eventData)
    {
        Vector2 pointerWorldPos = ScreenToWorldPoint(eventData.position);
        pointPos = new Vector2((pointerWorldPos.x - background.position.x) / ((background.rect.size.x - knob.rect.size.x) * background.parent.localScale.x / 2), 0);
        pointPos = (pointPos.magnitude > 1.0f) ? pointPos.normalized : pointPos;
    }

    private void MovePoint()
    {
        knob.transform.position = new Vector2((pointPos.x * ((background.rect.size.x - knob.rect.size.x) * background.parent.localScale.x / 2) * offset) + background.position.x, background.position.y);
    }

    /// <summary>
    /// 将屏幕坐标转换成世界坐标
    /// </summary>>
    /// <param name = "pos"></param>
    /// <returns></returns>
    public Vector2 ScreenToWorldPoint(Vector3 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
