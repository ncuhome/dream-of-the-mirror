using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilDownwardAttackState : DevilState
{
    public int attackDamage = 1;
    public LayerMask attackMask;
    public float firstJumpHeight;
    public float secondJumpHeight;
    public float thirdJumpHeight;
    public Vector2 firstAttackOffset;
    public Vector2 secondAttackOffset;
    public Vector2 thirdAttackOffset;

    private int jumpIndex = 0;
    // private Vector2 targetPoint;
    // private GameObject prefab1, prefab2;
    // private int impactTimes;
    // public float riseDuration;
    // private float nextDecisionTime;
    private bool rising;

    public override bool CanEnter(Devil devil)
    {
        if (jumpIndex > 2)
        {
            jumpIndex = 0;
            return false;
        }
        return true;
    }

    public override void Enter(Devil devil_, MoveCommand moveCommand)
    {
        base.Enter(devil_, moveCommand);
        jumpIndex++;
        GetEndTime("DownwardAttack");
        devil_.Anim_.Animator_.SetTrigger("DownwardAttack");
        // devil_.Physics_.SetAttack(attackDamage, attackMask);
        // devil_.Physics_.DownwardAttack(attackDamage, attackMask);
        // nextDecisionTime = Time.time;
    }

    // public override DevilState HandleCommand(MoveCommand moveCommand, ActionCommand actionCommand)
    // {
    //     return null;
    // }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        switch (jumpIndex)
        {
            case 1:
                devil_.Physics_.DownwardJump(jumpIndex, firstJumpHeight, stateDuration);
                break;
            case 2:
                devil_.Physics_.DownwardJump(jumpIndex, secondJumpHeight, stateDuration);
                break;
            case 3:
                devil_.Physics_.DownwardJump(jumpIndex, thirdJumpHeight, stateDuration);
                break;
        }
    }

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            switch (jumpIndex)
            {
                case 1:
                    devil_.Physics_.DownwardAttack(attackDamage, attackMask, firstAttackOffset);
                    devil_.TranslationState(downwardAttack);
                    break;
                case 2:
                    devil_.Physics_.DownwardAttack(attackDamage, attackMask, secondAttackOffset);
                    devil_.TranslationState(downwardAttack);
                    break;
                case 3:
                    devil_.Physics_.DownwardAttack(attackDamage, attackMask, thirdAttackOffset);
                    devil_.TranslationState(idling);
                    break;
            } 
        }
    }
}
