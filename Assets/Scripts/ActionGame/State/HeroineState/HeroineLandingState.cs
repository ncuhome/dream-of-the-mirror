using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineLandingState : HeroineState
{
    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        GetEndTime("GirlHero_Land");
        girlHero.Particle_.CreateDust();
        girlHero.Anim_.Animator_.SetTrigger("Land");
    }

    public override HeroineState HandleCommand(GirlHero girlHero, MoveCommand moveCommand, ActionCommand actionCommand)
    {
        if (moveCommand.type == MoveCommand.MoveType.repel)
        {
            return HeroineState.repelling;
        }

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

    public override void StateUpdate()
    {
        if (Time.time > stateTime)
        {
            girlHero_.TranslationState(idling);
        }
    }
}