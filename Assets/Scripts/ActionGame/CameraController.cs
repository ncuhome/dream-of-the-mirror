using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    public float smoothTime = 0.05f;
    public float posY = 1f;
    
    public float horizontalOffset = 1f;

    private MobileHorizontalInputController inputController;
    private float horizontalOffsetDir = 1f;

    void Start()
    {
        GameObject directionJoyStick = DirectionJoyStickManager.instance.directionJoyStick;
        inputController = directionJoyStick.GetComponent<MobileHorizontalInputController>();    
    }

    void Update()
    {
        // 虚拟轴水平移动
        if (inputController.dragging)
        {
            horizontalOffsetDir = inputController.horizontal;
        }
        else
        {
            horizontalOffsetDir = Input.GetAxisRaw("Horizontal");
        }


        Vector3 targetPos = new Vector3(target.transform.position.x + horizontalOffset * horizontalOffsetDir, target.transform.position.y + posY, -100);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothTime * 100);
    }
}