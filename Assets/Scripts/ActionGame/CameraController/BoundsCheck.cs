using UnityEngine;

//summary输入///开始
/// <summary>
/// 保持游戏对象在屏幕
/// 只对位于[0, 0, 0]的主正交摄像机有效
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float radius = 3f;
    public bool keepOnScreen = true; //是否将游戏对象强制保留在屏幕上

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    public Rigidbody2D rb;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;
    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void LateUpdate()
    {
        // if (!CameraController2.instance.targetIsInNewIndex)
        // {
        //     return;
        // }
        // Debug.Log("aaa");
        Vector3 pos = rb.position;
        Vector2 cameraPos = Camera.main.transform.position;
        isOnScreen = true;
        offDown = offUp = offLeft = offRight = false;

        if(pos.x > cameraPos.x + camWidth - radius)
        {
            pos.x = cameraPos.x + camWidth - radius;
            offRight = true;
        }

        if(pos.x < cameraPos.x - camWidth + radius)
        {
            pos.x = cameraPos.x - camWidth + radius;
            offLeft = true;
        }

        if(pos.y > cameraPos.y + camHeight - radius)
        {
            pos.y = cameraPos.y + camHeight - radius;
            offUp = true;
        }

        if(pos.y < cameraPos.y - camHeight + radius)
        {
            pos.y = cameraPos.y - camHeight + radius;
            offDown = true;
        }

        isOnScreen = !(offDown || offUp || offLeft || offRight);
        if(keepOnScreen && !isOnScreen)
        {
            // Debug.Log(pos);
            rb.MovePosition(pos);
            isOnScreen = true;
            offDown = offUp = offLeft = offRight = false;
        }
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
        {
            return;
        }
        Vector2 boundSize = new Vector2(camWidth * 2, camHeight * 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Camera.main.transform.position, boundSize);

    }
}
