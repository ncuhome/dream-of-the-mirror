using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineFloatingState : HeroineState
{
    public float moveSpeed;
    private int horizontal = 0;
    private int vertical = 0;

    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        girlHero.Anim_.Animator_.SetTrigger("Float");
    }

    public override HeroineState HandleCommand(GirlHero girlHero, MoveCommand moveCommand, ActionCommand actionCommand)
    {
        if (moveCommand.type == MoveCommand.MoveType.repel)
        {
            return HeroineState.repelling;
        }
        InitState(moveCommand);
        
        if (actionCommand is ActionCommand.Jump)
        {
            return HeroineState.jumping;
        }
        if (actionCommand is ActionCommand.Sword)
        {
            return HeroineState.swording;
        }
        if (actionCommand is ActionCommand.Shoot)
        {
            return HeroineState.shooting;
        }
        if (actionCommand is ActionCommand.Roll)
        {
            return HeroineState.rolling;
        }
        return null;
    }

    protected override void Awake()
    {
        
    }

    public override void StateFixedUpdate()
    {
        girlHero_.Physics_.HorizontalMove(horizontal, moveSpeed);
    }

    public override void StateUpdate()
    {
        if (girlHero_.Physics_.IsGrounded)
        {
            girlHero_.TranslationState(HeroineState.landing);
            return;
        }
    }

    private void InitState(MoveCommand moveCommand)
    {
        horizontal = (int)moveCommand.horizontal;
        vertical = (int)moveCommand.vertical;
    }
}