using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathIdleState : DeathState
{
    public float idleDuration;

    private float idleTime;

    public override void Enter(Death death_)
    {
        base.Enter(death_);
        death_.Anim_.Animator_.SetTrigger("Idle");
        idleTime = Time.time + idleDuration;
    }

    public override DeathState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        if (Time.time < idleTime)
        {
            return null;
        }
        // Debug.Log(actionCommand);
        if (actionCommand is ActionCommand.Sword)
        {
            return attacking;
        }
        if (actionCommand is ActionCommand.Shoot)
        {
            return shooting;
        }
        if (actionCommand is ActionCommand.Walk)
        {
            return walking;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        death_.Physics_.HorizontalMove(0, 0);
    }
}