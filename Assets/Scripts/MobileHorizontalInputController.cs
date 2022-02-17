using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]

//水平轴控制脚本，利用轴图片和背景图片的位移差结合unity中的轴系统获取竖直和水平的位移变化量
public class MobileHorizontalInputController : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Reference")]
    public RectTransform background;
    public RectTransform knob;

    [Header("Input Values")]
    public bool dragging = false;
    public float horizontal = 0;
    //控制轴可以移动的半径
    public float offset = 2f;

    private Vector2 pointPos;
    private Vector2 backgroundPos;

    void Start()
    {
        backgroundPos = background.position;    
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;

        Vector2 pointerWorldPos = ScreenToWorldPoint(eventData.position);
        pointPos = new Vector2((pointerWorldPos.x - background.position.x) / ((background.rect.size.x - knob.rect.size.x) * background.parent.localScale.x / 2), 0);
        pointPos = (pointPos.magnitude > 1.0f) ? pointPos.normalized : pointPos;

        knob.transform.position = new Vector2((pointPos.x * ((background.rect.size.x - knob.rect.size.x) * background.parent.localScale.x / 2) * offset) + background.position.x, background.position.y);
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

    void Update()
    {
        if (pointPos.x > 0)
        {
            horizontal = 1;
            return;
        }
        if (pointPos.x < 0)
        {
            horizontal = -1;
            return;
        }
        horizontal = 0;
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
