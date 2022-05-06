using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroineRunningState : HeroineState
{
    public float moveSpeed;
    public AudioSource runAudio;

    private int runDir;

    public override void Enter(GirlHero girlHero)
    {
        base.Enter(girlHero);
        girlHero.Particle_.CreateDust();
        girlHero.Anim_.Animator_.SetTrigger("Run");
        girlHero.PlayAudio(runAudio);
    }

    public override HeroineState HandleCommand(GirlHero girlHero, TranslationCommand translationCommand, Command buttonCommand)
    {
        if (translationCommand is RepelCommand)
        {
            return HeroineState.repelling;
        }

        runDir = (int)translationCommand.Horizontal;
        if (runDir == 0)
        {
            return HeroineState.idling;
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

    protected override void Start()
    {
        
    }

    public override void StateFixedUpdate()
    {
        girlHero_.Physics_.HorizontalMove(runDir, moveSpeed);
    }
}