using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilClaw3State : DevilState
{
    public int attackDamage = 1;
    public LayerMask attackMask;

    private float moveSpeed;
    private bool isWeak = false;
    private ActionCommand nextAction = ActionCommand.None;

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        base.Enter(devil_, moveCommand);
        GetEndTime("Claw2");
        devil_.Anim_.Animator_.SetTrigger("Attack");
        devil_.Physics_.SetAttack(attackDamage, attackMask);
    }

    public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        if (actionCommand == ActionCommand.Weak)
        {
            isWeak = true;
        }
        if (actionCommand == ActionCommand.Sword)
        {
            nextAction = ActionCommand.Sword;
        }
        else
        {
            nextAction = ActionCommand.Sprint;
        }
        // if (actionCommand == ActionCommand.Sprint)
        // {
        //     nextAction = ActionCommand.Sprint;
        // }
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
            if (nextAction == ActionCommand.Sword)
            {
                devil_.TranslationState(clawing4);
            }
            if (nextAction == ActionCommand.Sprint)
            {
                devil_.TranslationState(sprinting);
            }
        }
    }
}