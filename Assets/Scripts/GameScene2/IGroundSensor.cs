using UnityEngine;

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
