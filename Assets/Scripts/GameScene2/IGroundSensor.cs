using UnityEngine;

//地面触发器接口
public interface IGroundSensor
{
    Rigidbody2D M_rigidbody
    {
        get;
    }

    bool Is_DownJump_GroundCheck
    {
        get;
        set;
    }

    bool _IsGrounded
    {
        get;
        set;
    }

    int CurrentJumpCount
    {
        get;
        set;
    }
}
