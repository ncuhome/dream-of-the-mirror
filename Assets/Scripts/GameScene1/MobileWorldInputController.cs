using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]

//轴控制脚本，利用轴图片和背景图片的位移差结合unity中的轴系统获取竖直和水平的位移变化量
//使用前提是Canvas为世界坐标系渲染方式
public class MobileWorldInputController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform Background;
    public RectTransform Knob;

    [Header("Input Values")]
    public bool dragging = false;
    public float Horizontal = 0;
    public float Vertical = 0;
    //控制轴可以移动的半径
    public float offset;

    Vector2 PointPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;

        Vector2 pointerWorldPos = ScreenToWorldPoint(eventData.position);
        print(pointerWorldPos.x);
        print(Background.position.x);
        PointPosition = new Vector2((pointerWorldPos.x - Background.position.x) / ((Background.rect.size.x - Knob.rect.size.x) * Background.parent.localScale.x / 2), (pointerWorldPos.y - Background.position.y) / ((Background.rect.size.y - Knob.rect.size.y) * Background.parent.localScale.x / 2));
        PointPosition = (PointPosition.magnitude > 1.0f) ? PointPosition.normalized : PointPosition;

        Knob.transform.position = new Vector2((PointPosition.x * ((Background.rect.size.x - Knob.rect.size.x) * Background.parent.localScale.x / 2) * offset) + Background.position.x, (PointPosition.y * ((Background.rect.size.y - Knob.rect.size.y) * Background.parent.localScale.x / 2) * offset) + Background.position.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        PointPosition = new Vector2(0f, 0f);
        Knob.transform.position = Background.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndDrag(eventData);
    }

    void Update()
    {
        Horizontal = PointPosition.x;
        Vertical = PointPosition.y;
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
