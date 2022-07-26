using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilClaw1State : DevilState
{
    public int attackDamage = 1;
    public LayerMask attackMask;

    private bool isWeak = false;

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        base.Enter(devil_, moveCommand);
        GetEndTime("Claw1");
        devil_.Anim_.Animator_.SetTrigger("Attack");
        devil_.Physics_.Flip(moveCommand.horizontal > 0);
        devil_.Physics_.SetAttack(attackDamage, attackMask);
    }

    // public override void Exit()
    // {
    //     lastAttackTime = Time.time;
        // death_.Physics_.Flip(false);
    // }

    public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        if (actionCommand == ActionCommand.Weak)
        {
            isWeak = true;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    // public override void StateFixedUpdate()
    // {
    //     devil_.Physics_.HorizontalMove(0, 0);
    // }

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
                devil_.TranslationState(clawing2);
            }
        }
    }
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere((Vector2)transform.position + attackOffset, attackRadiu); 
    // }
}
