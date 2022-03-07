using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static float MIN_DIR_OFFSET = 0.05f;

    private MobileInputController mic;

    private void Awake()
    {
        GameObject directionJoyStick = DirectionJoyStickManager.instance.directionJoyStick;
        // 移动端或者测试环境下才显示虚拟摇杆
#if (UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR)
        mic = directionJoyStick.GetComponent<MobileInputController>(); //切换脚本需要改动
#else
        Destroy(directionJoyStick);
#endif
    }

    //提供给SceneController，使其改变人物移动方向
    public Direction GetDirection()
    {
        Direction dir = Direction.Idle;
        // 只编译对应平台需要的控制代码
#if UNITY_EDITOR
        if (mic.dragging)
        {
            dir = JudgeDirection(mic.horizontal, mic.vertical);
        }
        else
        {
            dir = JudgeDirection(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
#else
#if UNITY_IOS || UNITY_ANDROID
        if (mic.dragging)
        {
            dir = JudgeDirection(mic.horizontal, mic.vertical);
        }
#else
        dir = JudgeDirection(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif
#endif
        return dir;
    }

    private Direction JudgeDirection(float x, float y)
    {
        float absX = Mathf.Abs(x);
        float absY = Mathf.Abs(y);
        if (absX <= MIN_DIR_OFFSET && absY <= MIN_DIR_OFFSET)
        {
            return Direction.Idle;
        }
        if (absX > absY)
        {
            if (x > 0)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Left;
            }
        }
        else
        {
            if (y > 0)
            {
                return Direction.Up;
            }
            else
            {
                return Direction.Down;
            }
        }
    }

}
