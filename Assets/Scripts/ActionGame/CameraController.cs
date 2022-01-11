using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameObject target;

    public float smoothTime = 0.05f;
    public float posY = 1;

    private Vector3 cameraVelocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + posY, -100);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothTime * 100);
        rectTransform.position = (Vector2)transform.position;
    }
}