using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameObject target;

    public float smoothTime = 0.05f;
    public float posY = 1f;
    
    public float horizontalOffset = 1f;

    private MobileHorizontalInputController inputController;
    private float horizontalOffsetDir = 1f;
    private float tXDir = 0;

    void Start()
    {
        GameObject directionJoyStick = GameObject.FindGameObjectWithTag("DirectionJoyStick");
        inputController = directionJoyStick.GetComponent<MobileHorizontalInputController>();    
    }

    void FixedUpdate()
    {
        // 虚拟轴水平移动
        if (inputController.dragging)
        {
            tXDir = inputController.horizontal;
        }
        else
        {
            tXDir = Input.GetAxisRaw("Horizontal");
        }

        if (tXDir > 0)
        {
            horizontalOffsetDir = 1;
        }
        if (tXDir < 0)
        {
            horizontalOffsetDir = -1;
        }

        Vector3 targetPos = new Vector3(target.transform.position.x + horizontalOffset * horizontalOffsetDir, target.transform.position.y + posY, -100);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothTime * 100);
        rectTransform.position = (Vector2)transform.position;
    }
}