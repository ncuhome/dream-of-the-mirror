using UnityEngine;

// [System.Serializable]
// public struct CameraBorder
// {
//     public Vector2 leftTopPoint;
//     public Vector2 rightBottomPoint;
// };

public class ActionGameCameraController : MonoBehaviour
{
    public static ActionGameCameraController instance;
    public GameObject target;
    public float smoothTime = 0.05f;
    public float posY = 1f;
    public float horizontalOffset = 1f;

    private InputHandler inputHandler;
    // [Header("各场景摄像机边界")]
    // [SerializeField]
    // public CameraBorder[] cameraBorders;

    // [Header("摄像机所处的场景")]
    // public int index;

    private float camWidth;
    private float camHeight;

    // [Header("玩家是否处在当前生成场景中")]
    // public bool targetIsInNewIndex = true;

    // private MobileHorizontalInputController inputController;
    private float horizontalOffsetDir = 1f;

    void Start()
    {
        if (target == null)
        {
            target = PlayerManager.instance.girlHero;
        }

        inputHandler = InputHandlerManager.instance.inputHandler;

        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;   
        // index = 0;
    }

    void Update()
    {
        // GetDir();
        // GetIndex();
        CameraMove();
    }

    // private void GetDir()
    // {
    //     // 虚拟轴水平移动
    //     if (inputController.dragging)
    //     {
    //         if (inputController.horizontal != 0)
    //         {
    //             horizontalOffsetDir = inputController.horizontal;
    //         }
    //     }
    //     else
    //     {
    //         if (Input.GetAxisRaw("Horizontal") != 0)
    //         {
    //             horizontalOffsetDir = Input.GetAxisRaw("Horizontal");
    //         }
    //     }
    // }

    // private void GetIndex()
    // {
    //     index =  PlayerManager.instance.girlHero.GetIndex;
    // }

    private void CameraMove()
    {
        Vector3 targetPos;
        
        IndexBorder indexBorder = AreaManager.instance.GetBorder(0);
        
        targetPos = new Vector3(0, 0, -100);
        targetPos.x = Mathf.Clamp(target.transform.position.x + horizontalOffset * horizontalOffsetDir, indexBorder.leftTopPoint.x, indexBorder.rightBottomPoint.x);
        targetPos.y = 1.33f;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothTime * 100);
    }
}