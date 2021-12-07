using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public RectTransform JoyStickAndButtonCanvasRectTransform;

    public GameObject Target;
    public float Smoothvalue = 2f;
    public float PosY = 1;

    void Update()
    {
        Vector3 Targetpos = new Vector3(Target.transform.position.x, Target.transform.position.y + PosY, -100);
        transform.position = Vector3.Lerp(transform.position, Targetpos, Time.deltaTime * Smoothvalue);
        JoyStickAndButtonCanvasRectTransform.position = (Vector2)transform.position;
    }
}
