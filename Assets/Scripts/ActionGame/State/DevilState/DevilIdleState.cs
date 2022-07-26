using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilIdleState : DevilState
{
    public float idleDuration;
    public int attackDamage = 0;
    public LayerMask attackMask;


    private float idleTime;

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        base.Enter(devil_, moveCommand);
        devil_.Anim_.Animator_.SetTrigger("Idle");
        idleTime = Time.time + idleDuration;
        devil_.Physics_.SetAttack(attackDamage, attackMask);
    }

    public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        if (actionCommand is ActionCommand.Weak)
        {
            return downwardAttack;
        }
        if (Time.time < idleTime)
        {
            return null;
        }
        if (actionCommand is ActionCommand.Sword)
        {
            return clawing1;
        }
        if (actionCommand is ActionCommand.Shoot)
        {
            return shooting;
        }
        if (actionCommand is ActionCommand.Sprint)
        {
            return sprinting;
        }
        if (actionCommand is ActionCommand.Walk)
        {
            return running;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    // public override void StateFixedUpdate()
    // {
    //     devil_.Physics_.HorizontalMove(0);
    // }
}
