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

    public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
    {
        if (translationCommand is RepelCommand)
        {
            return HeroineState.repelling;
        }

        if (buttonCommand is JumpCommand)
        {
            return HeroineState.jumping;
        }
        if (buttonCommand is SwordCommand)
        {
            return HeroineState.swording;
        }
        if (buttonCommand is ShootCommand)
        {
            return HeroineState.shooting;
        }
        if (buttonCommand is RollCommand)
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