using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWalkingState : DeathState
{
    public float walkSpeed;

    private int walkDir;

    public override void Enter(Death death_)
    {
        base.Enter(death_);
        death_.Anim_.Animator_.SetTrigger("Walk");
    }

    public override DeathState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        walkDir = (int)moveCommand.horizontal;
        if (actionCommand is ActionCommand.Sword)
        {
            return attacking;
        }
        if (actionCommand is ActionCommand.Shoot)
        {
            return shooting;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        death_.Physics_.HorizontalMove(walkDir, walkSpeed);
    }
}