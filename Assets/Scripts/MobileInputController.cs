using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]

//轴控制脚本，利用轴图片和背景图片的位移差结合unity中的轴系统获取竖直和水平的位移变化量
//使用前提是Canvas为世界坐标系渲染方式
public class MobileInputController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform knob;

    [Header("Input Values")]
    public bool dragging = false;
    public float horizontal = 0;
    public float vertical = 0;
    //控制轴可以移动的半径
    public float offset;

    Vector2 pointPos;

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;

        Vector2 pointerWorldPos = ScreenToWorldPoint(eventData.position);
        print(pointerWorldPos.x);
        print(background.position.x);
        pointPos = new Vector2((pointerWorldPos.x - background.position.x) / ((background.rect.size.x - knob.rect.size.x) * background.parent.localScale.x / 2), (pointerWorldPos.y - background.position.y) / ((background.rect.size.y - knob.rect.size.y) * background.parent.localScale.x / 2));
        pointPos = (pointPos.magnitude > 1.0f) ? pointPos.normalized : pointPos;

        knob.transform.position = new Vector2((pointPos.x * ((background.rect.size.x - knob.rect.size.x) * background.parent.localScale.x / 2) * offset) + background.position.x, (pointPos.y * ((background.rect.size.y - knob.rect.size.y) * background.parent.localScale.x / 2) * offset) + background.position.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        pointPos = new Vector2(0f, 0f);
        knob.transform.position = background.position;
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
        horizontal = pointPos.x;
        vertical = pointPos.y;
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
