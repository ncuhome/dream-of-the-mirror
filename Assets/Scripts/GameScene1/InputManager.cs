using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private MobileInputController mic;

    private void Awake()
    {
        GameObject directionJoyStick = GameObject.Find("DirectionJoyStick");
        // 移动端或者测试环境下才显示虚拟摇杆
#if (UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR)
        mic = directionJoyStick.GetComponent<MobileInputController>();
#else
        Destroy(directionJoyStick);
#endif
    }

    public Direction GetDirection()
    {
        Direction dir = Direction.Idle;
        // 只编译对应平台需要的控制代码
#if UNITY_EDITOR
        if (mic.dragging)
        {
            dir = JudgeDirection(mic.Horizontal, mic.Vertical);
        }
        else
        {
            dir = JudgeDirection(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        }
#else
#if UNITY_IOS || UNITY_ANDROID
        if (mic.dragging)
        {
            dir = JudgeDirection(mic.Horizontal, mic.Vertical);
        }
#else
        dir = JudgeDirection(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif
#endif
        return dir;
    }

    private Direction JudgeDirection(float x, float y)
    {
        if (Mathf.Abs(x) <= 0.05f && Mathf.Abs(y) <= 0.05f)
            return Direction.Idle;
        if (x >= 0 && y >= 0)
        {
            if (x >= y)
                return Direction.Right;
            else
                return Direction.Up;
        }
        if (x >= 0 && y < 0)
        {
            if (x >= Mathf.Abs(y))
                return Direction.Right;
            else
                return Direction.Down;
        }
        if (x < 0 && y >= 0)
        {
            if (Mathf.Abs(x) >= y)
                return Direction.Left;
            else
                return Direction.Up;
        }
        if (x < 0 && y < 0)
        {
            if (x <= y)
                return Direction.Left;
            else
                return Direction.Down;
        }
        return Direction.Idle;
    }

}
