using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]

//轴控制脚本，利用轴图片和背景图片的位移差结合unity中的轴系统获取竖直和水平的位移变化量
//使用前提是Canvas的尺寸为1，1，1
public class MobileInputController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
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
        // print(eventData.position);
        // print(Background.position);

        PointPosition = new Vector2((eventData.position.x - Background.position.x) / ((Background.rect.size.x - Knob.rect.size.x) / 2), (eventData.position.y - Background.position.y) / ((Background.rect.size.y - Knob.rect.size.y) / 2));

        PointPosition = (PointPosition.magnitude > 1.0f) ? PointPosition.normalized : PointPosition;

        Knob.transform.position = new Vector2((PointPosition.x * ((Background.rect.size.x - Knob.rect.size.x) / 2) * offset) + Background.position.x, (PointPosition.y * ((Background.rect.size.y - Knob.rect.size.y) / 2) * offset) + Background.position.y);
        // float t = (PointPosition.x *((Background.rect.size.x-Knob.rect.size.x)/2)*offset) + Background.position.x;
        // float t = Background.rect.size.x-Knob.rect.size.x;
        // print(t);
        // print(Knob.transform.position);
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

    // public Vector2 Coordinate()
    // {
    //     return new Vector2(Horizontal, Vertical);
    // }
}
