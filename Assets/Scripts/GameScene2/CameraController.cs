using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    // JoyStickAndButtonCanvasRectTransform
    public RectTransform rectTransform;

    public GameObject target;
    public float smoothTime = 0.05f;
    public float posY = 1;

    private Vector3 cameraVelocity = Vector3.zero;

    void Update()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + posY, -100);
        transform.position = targetPos;
        rectTransform.position = (Vector2)transform.position;
    }
}