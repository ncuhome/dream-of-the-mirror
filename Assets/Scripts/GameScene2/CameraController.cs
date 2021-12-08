using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    // JoyStickAndButtonCanvasRectTransform
    public RectTransform rectTransform;

    public GameObject target;
    public float smoothFactor = 5f;
    public float posY = 1;

    void Update()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + posY, -100);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothFactor);
        rectTransform.position = (Vector2)transform.position;
    }
}
