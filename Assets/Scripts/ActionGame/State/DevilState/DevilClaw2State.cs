using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilClaw2State : DevilState
{
    public int attackDamage = 1;
    public float moveSpeed;
    public LayerMask attackMask;

    private bool isWeak = false;
    private int walkDir;

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        base.Enter(devil_, moveCommand);
        GetEndTime("Claw2");
        devil_.Anim_.Animator_.SetTrigger("Attack");
        devil_.Physics_.Flip(moveCommand.horizontal > 0);
        devil_.Physics_.SetAttack(attackDamage, attackMask);
    }

    public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        walkDir = (int)moveCommand.horizontal;
        if (actionCommand == ActionCommand.Weak)
        {
            isWeak = true;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        devil_.Physics_.ForwardMove(moveSpeed);
    }

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            if (isWeak)
            {
                devil_.TranslationState(downwardAttack);
            }
            else
            {
                devil_.TranslationState(clawing3);
            }
        }
    }
}
