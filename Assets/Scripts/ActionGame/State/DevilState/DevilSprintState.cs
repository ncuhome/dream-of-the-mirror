using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilSprintState : DevilState
{
    public float sprintSpeed;
    public int attackDamage = 1;
    public LayerMask attackMask;
    public float maxSprintDuration = 1f;

    // private int sprintDir;

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        stateTime = Time.time + maxSprintDuration;
        base.Enter(devil_, moveCommand);
        devil_.Anim_.Animator_.SetTrigger("Sprint");
        devil_.Physics_.Flip(moveCommand.horizontal > 0);
        devil_.Physics_.SetAttack(attackDamage, attackMask);
    }

    public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    {
        // sprintDir = (int)moveCommand.horizontal;
        if (actionCommand is ActionCommand.Weak)
        {
            return downwardAttack;
        }
        if (actionCommand is ActionCommand.Sword)
        {
            return uppercut;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        devil_.Physics_.ForwardMove(sprintSpeed);
    }

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            devil_.TranslationState(uppercut);
        }
    }
}
