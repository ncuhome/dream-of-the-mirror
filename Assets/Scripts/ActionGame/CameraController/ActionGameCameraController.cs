using System.Collections;
using UnityEngine;

// [System.Serializable]
// public struct CameraBorder
// {
//     public Vector2 leftTopPoint;
//     public Vector2 rightBottomPoint;
// };

public class ActionGameCameraController : MonoBehaviour
{
    // public static ActionGameCameraController instance;
    public GameObject target;
    public float smoothTime = 0.05f;
    public float horizontalOffsetPre = 0.25f;

    private float camWidth;
    private float camHeight;
    private InputHandler inputHandler;
    private IndexBorder indexBorder;
    private float verticalOffset;
    private float horizontalOffset;
    private float horizontalOffsetDir = 0f;
    private int currentIndex;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void Start()
    {
        if (target == null)
        {
            target = PlayerManager.instance.girlHero;
        }
        horizontalOffset = camWidth * horizontalOffsetPre;   
        inputHandler = InputHandlerManager.instance.inputHandler;

        InitCamPos();
    }

    void Update()
    {
        // GetIndex();
        CameraMove();
    }

    private void InitCamPos()
    {
        indexBorder = AreaManager.instance.GetBorder();
        currentIndex = AreaManager.instance.CurrentIndex;
        Vector3 tPos = new Vector3(0, 0, -100);
        tPos.x =  Mathf.Clamp(target.transform.position.x + horizontalOffset*horizontalOffsetDir, indexBorder.leftTopPoint.x+camWidth, indexBorder.rightBottomPoint.x-camWidth);
        tPos.y = Mathf.Clamp(target.transform.position.y, indexBorder.rightBottomPoint.y+camHeight, indexBorder.leftTopPoint.y-camHeight);
        transform.position = tPos;
    }

    private void CameraMove()
    {
        Vector3 targetPos = new Vector3(0, 0, -100);
        if (currentIndex != AreaManager.instance.CurrentIndex)
        {
            InitCamPos();
            return;
        }
        // indexBorder = AreaManager.instance.GetBorder();
        targetPos.x = CalCameraMoveX();
        targetPos.y = CalCameraMoveY();
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothTime * 100);
    }

    private float CalCameraMoveX()
    {
        horizontalOffsetDir = (int)InputHandlerManager.instance.inputHandler.HandleJoyStickInput().horizontal;
        if (horizontalOffsetDir == 0)
        {
            return transform.position.x;
        }   
        else
        {
            return Mathf.Clamp(target.transform.position.x + horizontalOffset * horizontalOffsetDir, indexBorder.leftTopPoint.x+camWidth, indexBorder.rightBottomPoint.x-camWidth);
        }     
    }

    private float CalCameraMoveY()
    {
        if (transform.position.y > indexBorder.rightBottomPoint.y+camHeight - 0.1 
        && transform.position.y < indexBorder.rightBottomPoint.y+camHeight + 0.1)
        {
            if (target.transform.position.y <= indexBorder.rightBottomPoint.y + 1.8*Camera.main.orthographicSize)
            {
                return transform.position.y;
            }
        }
        return Mathf.Clamp(target.transform.position.y, indexBorder.rightBottomPoint.y+camHeight, indexBorder.leftTopPoint.y-camHeight);
    }

    public void CameraShake(float duration,float strength)
    {
        StopCoroutine(Shake(duration, strength));
        StartCoroutine(Shake(duration, strength));
    }

    IEnumerator Shake(float duration,float strength)
    {
        Vector3 startPos = transform.position;
        while (duration > 0)
        {
            transform.position = Random.insideUnitSphere * strength+startPos;
            duration -= Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
    }
}