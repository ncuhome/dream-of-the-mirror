using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilShootState : DevilState
{
    [Header("Death刀气预制件")]
    public Bullet bullet;
    public float shootCd;
    public int attackDamage = 0;
    public LayerMask attackMask;

    private int shootDir;
    private float lastShootTime;
    private bool isWeak = false;

    public override bool CanEnter(Devil devil)
    {
        if (Time.time < lastShootTime + shootCd)
        {
            return false;
        }
        return true;
    }

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        base.Enter(devil_, moveCommand);
        GetEndTime("Remote");
        devil_.Anim_.Animator_.SetTrigger("Shoot");
        devil_.Physics_.Flip(moveCommand.horizontal > 0);
    }

    public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        shootDir = (int)moveCommand.horizontal;
        if (actionCommand == ActionCommand.Weak)
        {
            isWeak = true;
        }
        return null;
    }

    public override void Exit()
    {
        lastShootTime = Time.time;
        Quaternion bulletRotation = devil_.transform.rotation;
        bulletRotation.y = (devil_.transform.rotation.y == 0) ? 0 : 180;
        Instantiate(bullet, devil_.transform.position + devil_.transform.right * (-1), bulletRotation);
    }

    protected override void Awake()
    {
        
    }

    public override void StateUpdate()
    {
        devil_.Physics_.Rb.velocity = Vector2.zero;
        if (Time.time > stateTime)
        {
            if (isWeak)
            {
                devil_.TranslationState(downwardAttack);
            }
            else
            {
                devil_.TranslationState(idling);
            }
        }
    }

    // public override void StateFixedUpdate()
    // {
    //     devil_.Physics_.ShootMove(shootDir, 0);
    // }
}
