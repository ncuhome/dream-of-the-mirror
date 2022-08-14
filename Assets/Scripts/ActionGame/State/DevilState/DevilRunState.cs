using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilRunState : DevilState
{
    public float walkSpeed;
    public int attackDamage = 0;
    public LayerMask attackMask;

    private int walkDir;

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        base.Enter(devil_, moveCommand);
        devil_.Anim_.Animator_.SetTrigger("Run");
    }

    public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        walkDir = (int)moveCommand.horizontal;
        if (actionCommand is ActionCommand.Weak)
        {
            return downwardAttack;
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
        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        devil_.Physics_.RunMove(walkDir, walkSpeed);
    }
}
